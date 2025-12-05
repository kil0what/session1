using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace SportsGoodsApp
{
    public partial class Form1 : Form
    {
        // Элементы управления уже объявлены в Form1.Designer.cs
        // НЕ объявляйте их здесь снова!

        private Timer blockTimer;
        private string generatedCaptcha;
        private bool captchaRequired = false;
        private int blockSecondsRemaining = 0;
        private int failedAttempts = 0;
        private DateTime? blockedUntil = null;

        public Form1()
        {
            InitializeComponent();

            // Инициализируем таймер
            blockTimer = new Timer();
            blockTimer.Interval = 1000;
            blockTimer.Tick += BlockTimer_Tick;

            // Настройка формы
            SetupForm();
        }

        private void SetupForm()
        {
            // Настройка стилей
            this.Text = "ООО Спортивные товары - Вход";
            this.BackColor = Color.White;
            this.Font = new Font("Comic Sans MS", 10);

            // Настройка кнопок
            btnLogin.BackColor = Color.FromArgb(73, 140, 81);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Comic Sans MS", 10, FontStyle.Bold);

            btnGuest.BackColor = Color.FromArgb(118, 227, 131);
            btnGuest.ForeColor = Color.Black;
            btnGuest.FlatStyle = FlatStyle.Flat;
            btnGuest.Font = new Font("Comic Sans MS", 10);

            btnRefreshCaptcha.BackColor = Color.LightGray;
            btnRefreshCaptcha.FlatStyle = FlatStyle.Flat;

            // Скрываем CAPTCHA при запуске
            picCaptcha.Visible = false;
            txtCaptcha.Visible = false;
            btnRefreshCaptcha.Visible = false;
        }

        // ВХОД
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;
            string captcha = captchaRequired ? txtCaptcha.Text.Trim() : null;

            // Проверка заполнения
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка CAPTCHA
            if (captchaRequired && string.IsNullOrEmpty(captcha))
            {
                MessageBox.Show("Введите CAPTCHA", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка блокировки
            if (blockedUntil.HasValue && blockedUntil > DateTime.Now)
            {
                int seconds = (int)(blockedUntil.Value - DateTime.Now).TotalSeconds;
                MessageBox.Show($"Аккаунт заблокирован. Осталось: {seconds} сек",
                    "Блокировка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка CAPTCHA
            if (captchaRequired && captcha != generatedCaptcha)
            {
                MessageBox.Show("Неверная CAPTCHA", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                GenerateCaptcha();
                return;
            }

            // Проверка логина/пароля
            if (CheckLogin(login, password))
            {
                // Успешный вход - открываем главное окно
                OpenMainWindow();
            }
            else
            {
                failedAttempts++;

                if (failedAttempts >= 2)
                {
                    // Блокировка на 10 секунд
                    blockedUntil = DateTime.Now.AddSeconds(10);
                    blockSecondsRemaining = 10;
                    StartBlockTimer();

                    MessageBox.Show("Неверный логин или пароль. Аккаунт заблокирован на 10 секунд.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (failedAttempts == 1)
                {
                    // Показать CAPTCHA
                    captchaRequired = true;
                    ShowCaptchaControls();

                    MessageBox.Show("Неверный логин или пароль. Требуется CAPTCHA.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool CheckLogin(string login, string password)
        {
            try
            {
                // Проверяем подключение к базе данных (синхронно)
                bool canConnect = DatabaseHelper.TestConnection();

                if (!canConnect)
                {
                    MessageBox.Show("Не удалось подключиться к базе данных.\nПроверьте:\n1. Установлен ли SQL Server LocalDB\n2. Существует ли база SportsGoodsDB_Cyrillic\n3. Выполнен ли скрипт создания БД",
                        "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Импортируем пользователей если таблица пустая
                DatabaseHelper.ImportUsersIfNeeded();

                // Пробуем авторизоваться через базу данных
                var user = DatabaseHelper.AuthenticateUser(login, password);

                if (user != null)
                {
                    Session.CurrentUser = user;

                    // Логирование для отладки
                    Console.WriteLine($"Успешный вход: {user.FullName}, Роль: {user.Role}");

                    // Можно добавить логирование в файл
                    try
                    {
                        string logMessage = $"{DateTime.Now}: Успешный вход - {user.FullName} ({user.Role})";
                        File.AppendAllText("login_log.txt", logMessage + Environment.NewLine);
                    }
                    catch { /* Игнорируем ошибки логирования */ }

                    return true;
                }
                else
                {
                    // Неудачная попытка входа
                    Console.WriteLine($"Неудачная попытка входа: {login}");

                    // Логирование неудачных попыток
                    try
                    {
                        string logMessage = $"{DateTime.Now}: Неудачная попытка входа - {login}";
                        File.AppendAllText("login_log.txt", logMessage + Environment.NewLine);
                    }
                    catch { /* Игнорируем ошибки логирования */ }

                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}\nДетали: {ex.InnerException?.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void BtnGuest_Click(object sender, EventArgs e)
        {
            Session.CurrentUser = new User
            {
                Id = 0,
                Login = "guest",
                Role = "Guest",
                FullName = "Гость"
            };

            OpenMainWindow();
        }

        private void OpenMainWindow()
        {
            // Закрываем текущее окно входа
            this.Hide();

            // Открываем главное окно
            MainForm mainForm = new MainForm();
            mainForm.FormClosed += (s, args) => this.Close();
            mainForm.Show();
        }

        // ГЕНЕРАЦИЯ CAPTCHA (исправленная)
        private void GenerateCaptcha()
        {
            if (picCaptcha == null) return;

            Random rand = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            generatedCaptcha = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[rand.Next(s.Length)]).ToArray());

            Bitmap bmp = new Bitmap(picCaptcha.Width, picCaptcha.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                Font font = new Font("Comic Sans MS", 20, FontStyle.Bold);

                // Рисуем символы
                for (int i = 0; i < 4; i++)
                {
                    int x = 10 + i * 40 + rand.Next(-10, 10);
                    int y = 20 + rand.Next(-10, 10);

                    g.TranslateTransform(x, y);
                    float angle = rand.Next(-30, 30);
                    g.RotateTransform(angle);

                    // Рисуем символ
                    g.DrawString(generatedCaptcha[i].ToString(), font, Brushes.Black, 0, 0);

                    // Перечеркивание (50% шанс)
                    if (rand.Next(2) == 0)
                    {
                        g.DrawLine(new Pen(Color.Red, 1), -5, 20, 30, -5);
                    }

                    g.ResetTransform();
                }

                // Графический шум - линии
                for (int i = 0; i < 20; i++)
                {
                    g.DrawLine(
                        new Pen(Color.FromArgb(rand.Next(150, 200), rand.Next(150, 200), rand.Next(150, 200)), 1),
                        rand.Next(bmp.Width), rand.Next(bmp.Height),
                        rand.Next(bmp.Width), rand.Next(bmp.Height)
                    );
                }

                // Точечный шум
                for (int i = 0; i < 300; i++)
                {
                    bmp.SetPixel(
                        rand.Next(bmp.Width),
                        rand.Next(bmp.Height),
                        Color.FromArgb(rand.Next(150, 200), rand.Next(150, 200), rand.Next(150, 200))
                    );
                }
            }

            if (picCaptcha.Image != null)
                picCaptcha.Image.Dispose();

            picCaptcha.Image = bmp;
        }

        // ПОКАЗАТЬ CAPTCHA
        private void ShowCaptchaControls()
        {
            picCaptcha.Visible = true;
            txtCaptcha.Visible = true;
            btnRefreshCaptcha.Visible = true;

            txtCaptcha.Text = "";
            txtCaptcha.Focus();

            GenerateCaptcha();
        }

        // СКРЫТЬ CAPTCHA
        private void HideCaptchaControls()
        {
            picCaptcha.Visible = false;
            txtCaptcha.Visible = false;
            txtCaptcha.Text = "";
            btnRefreshCaptcha.Visible = false;
        }

        // ОБНОВИТЬ CAPTCHA
        private void BtnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }

        // ТАЙМЕР БЛОКИРОВКИ
        private void BlockTimer_Tick(object sender, EventArgs e)
        {
            blockSecondsRemaining--;

            if (blockSecondsRemaining <= 0)
            {
                blockTimer.Stop();
                EnableControls();
                this.Text = "ООО Спортивные товары - Вход";
                lblBlockTimer.Text = "";

                MessageBox.Show("Блокировка снята", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.Text = $"Блокировка: {blockSecondsRemaining} сек";
                lblBlockTimer.Text = $"Блокировка: {blockSecondsRemaining} сек";
            }
        }

        private void StartBlockTimer()
        {
            txtLogin.Enabled = false;
            txtPassword.Enabled = false;
            txtCaptcha.Enabled = false;
            btnLogin.Enabled = false;
            btnGuest.Enabled = false;
            btnRefreshCaptcha.Enabled = false;

            blockTimer.Start();
        }

        private void EnableControls()
        {
            txtLogin.Enabled = true;
            txtPassword.Enabled = true;
            txtCaptcha.Enabled = true;
            btnLogin.Enabled = true;
            btnGuest.Enabled = true;
            btnRefreshCaptcha.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (picCaptcha != null && picCaptcha.Image != null)
                picCaptcha.Image.Dispose();

            blockTimer?.Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {
}

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}