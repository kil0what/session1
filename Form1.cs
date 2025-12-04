using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public partial class Form1 : Form
    {
        private Timer blockTimer;
        private string generatedCaptcha;
        private bool captchaRequired = false;
        private int blockSecondsRemaining = 0;
        private int failedAttempts = 0;
        private DateTime? blockedUntil = null;
        private PictureBox logoPictureBox;

        public Form1()
        {
            InitializeComponent();

            // УБИРАЕМ SetupForm() отсюда - он конфликтует с дизайнером
            // SetupForm();

            LoadLogo(); // Оставляем только логотип
            SetupEventHandlers(); // Добавляем обработчики
            ApplyStyles(); // Применяем стили БЕЗ изменения позиций
        }

        private void LoadLogo()
        {
            try
            {
                // Создаем PictureBox для логотипа
                logoPictureBox = new PictureBox
                {
                    Size = new Size(200, 60),
                    Location = new Point((this.ClientSize.Width - 200) / 2, 20),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Anchor = AnchorStyles.Top
                };

                // Если файл логотипа существует, загружаем его
                if (System.IO.File.Exists("Resources/logo.png"))
                {
                    logoPictureBox.Image = Image.FromFile("Resources/logo.png");
                }
                else
                {
                    //
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки логотипа: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SetupEventHandlers()
        {
            // Привязываем обработчики к кнопкам
            btnLogin.Click += BtnLogin_Click;
            btnGuest.Click += BtnGuest_Click;
            btnRefreshCaptcha.Click += BtnRefreshCaptcha_Click;

            // Скрываем CAPTCHA при запуске
            picCaptcha.Visible = false;
            txtCaptcha.Visible = false;
            btnRefreshCaptcha.Visible = false;

            // Инициализируем таймер
            blockTimer = new Timer();
            blockTimer.Interval = 1000;
            blockTimer.Tick += BlockTimer_Tick;
        }

        private void ApplyStyles()
        {
            // Применяем только стили, НЕ меняем расположение!

            // Форма
            this.Text = "ООО Спортивные товары - Вход";
            this.Font = new Font("Comic Sans MS", 10);
            this.BackColor = Color.White;

            // Кнопка Войти
            btnLogin.BackColor = Color.FromArgb(73, 140, 81); // Акцентный цвет
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Comic Sans MS", 10, FontStyle.Bold);

            // Кнопка Гость
            btnGuest.BackColor = Color.FromArgb(118, 227, 131); // Дополнительный цвет
            btnGuest.ForeColor = Color.Black;
            btnGuest.FlatStyle = FlatStyle.Flat;
            btnGuest.Font = new Font("Comic Sans MS", 10);

            // Кнопка обновления CAPTCHA
            btnRefreshCaptcha.BackColor = Color.LightGray;
            btnRefreshCaptcha.FlatStyle = FlatStyle.Flat;

            // Поля ввода
            txtLogin.Font = new Font("Comic Sans MS", 10);
            txtPassword.Font = new Font("Comic Sans MS", 10);
            txtCaptcha.Font = new Font("Comic Sans MS", 10);
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
            // Ищем пользователя в базе
            var user = FakeDatabase.Users.FirstOrDefault(u =>
                u.Login.Equals(login, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);

            if (user != null)
            {
                Session.CurrentUser = new User
                {
                    Id = user.Id,
                    Login = user.Login,
                    Role = user.Role,
                    FullName = user.FullName
                };
                return true;
            }

            // Также проверяем старые тестовые аккаунты (можно удалить после тестирования)
            if (login == "admin" && password == "admin123")
            {
                Session.CurrentUser = new User
                {
                    Id = 1,
                    Login = "admin",
                    Role = "Admin",
                    FullName = "Пахомова Аиша Анатольевна"
                };
                return true;
            }
            if (login == "manager" && password == "manager123")
            {
                Session.CurrentUser = new User
                {
                    Id = 4,
                    Login = "manager",
                    Role = "Manager",
                    FullName = "Григорьева Арина Арсентьевна"
                };
                return true;
            }
            if (login == "client" && password == "client123")
            {
                Session.CurrentUser = new User
                {
                    Id = 7,
                    Login = "client",
                    Role = "Client",
                    FullName = "Поляков Степан Егорович"
                };
                return true;
            }

            return false;
        }
        // ГОСТЬ
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
            // Пустой метод
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Пустой метод
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Можно оставить пустым или добавить инициализацию
        }
    }
}