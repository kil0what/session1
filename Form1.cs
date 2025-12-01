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

        public Form1()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Настройка таймера
            blockTimer = new Timer();
            blockTimer.Interval = 1000;
            blockTimer.Tick += BlockTimer_Tick;

            // Стили
            this.Text = "ООО Спортивные товары - Вход";
            this.Font = new Font("Comic Sans MS", 10);
            this.BackColor = Color.White;
            this.Size = new Size(400, 450);

            // Кнопка Войти
            btnLogin.BackColor = Color.FromArgb(73, 140, 81);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;

            // Кнопка Гость
            btnGuest.BackColor = Color.FromArgb(118, 227, 131);
            btnGuest.ForeColor = Color.Black;
            btnGuest.FlatStyle = FlatStyle.Flat;

            // Скрыть CAPTCHA при запуске
            if (txtCaptcha != null) txtCaptcha.Visible = false;
            if (picCaptcha != null) picCaptcha.Visible = false;
            if (btnRefreshCaptcha != null) btnRefreshCaptcha.Visible = false;

            // Обработчики
            btnLogin.Click += BtnLogin_Click;
            btnGuest.Click += BtnGuest_Click;
            if (btnRefreshCaptcha != null)
                btnRefreshCaptcha.Click += BtnRefreshCaptcha_Click;
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
                MessageBox.Show($"Аккаунт заблокирован. Осталось: {(blockedUntil.Value - DateTime.Now).Seconds} сек",
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
                MessageBox.Show("Вход выполнен успешно!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                failedAttempts = 0;
                captchaRequired = false;
                HideCaptchaControls();
            }
            else
            {
                failedAttempts++;

                if (failedAttempts >= 2)
                {
                    // Блокировка
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

        // Проверка логина/пароля (заглушка)
        private bool CheckLogin(string login, string password)
        {
            if (login == "admin" && password == "admin123")
            {
                Session.CurrentUser = new User { Id = 1, Login = "admin", Role = "Admin", FullName = "Администратор" };
                return true;
            }
            if (login == "manager" && password == "manager123")
            {
                Session.CurrentUser = new User { Id = 2, Login = "manager", Role = "Manager", FullName = "Менеджер" };
                return true;
            }
            if (login == "client" && password == "client123")
            {
                Session.CurrentUser = new User { Id = 3, Login = "client", Role = "Client", FullName = "Клиент" };
                return true;
            }
            return false;
        }

        // ГОСТЬ
        private void BtnGuest_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Добро пожаловать в гостевой режим!\nВы можете просматривать товары без авторизации.",
                "Гость", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ДОБАВЬТЕ ЭТОТ МЕТОД (если его нет):
        private void BtnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }

        // ПОКАЗАТЬ CAPTCHA
        private void ShowCaptchaControls()
        {
            if (txtCaptcha != null) txtCaptcha.Visible = true;
            if (picCaptcha != null) picCaptcha.Visible = true;
            if (btnRefreshCaptcha != null) btnRefreshCaptcha.Visible = true;

            GenerateCaptcha();
        }

        // СКРЫТЬ CAPTCHA
        private void HideCaptchaControls()
        {
            if (txtCaptcha != null)
            {
                txtCaptcha.Visible = false;
                txtCaptcha.Text = "";
            }
            if (picCaptcha != null) picCaptcha.Visible = false;
            if (btnRefreshCaptcha != null) btnRefreshCaptcha.Visible = false;
        }

        // ГЕНЕРАЦИЯ CAPTCHA
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
                    int x = 10 + i * 30 + rand.Next(-5, 5);
                    int y = 10 + rand.Next(-5, 5);

                    g.TranslateTransform(x, y);
                    g.RotateTransform(rand.Next(-30, 30));
                    g.DrawString(generatedCaptcha[i].ToString(), font, Brushes.Black, 0, 0);
                    g.ResetTransform();

                    // Линии
                    g.DrawLine(Pens.Gray, x - 5, y + 15, x + 25, y + 15);
                }

                // Шум
                for (int i = 0; i < 100; i++)
                {
                    int x = rand.Next(bmp.Width);
                    int y = rand.Next(bmp.Height);
                    bmp.SetPixel(x, y, Color.FromArgb(rand.Next(100, 200), rand.Next(100, 200), rand.Next(100, 200)));
                }
            }

            if (picCaptcha.Image != null)
                picCaptcha.Image.Dispose();

            picCaptcha.Image = bmp;
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
                MessageBox.Show("Блокировка снята", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.Text = $"Блокировка: {blockSecondsRemaining} сек";
            }
        }

        private void StartBlockTimer()
        {
            txtLogin.Enabled = false;
            txtPassword.Enabled = false;
            if (txtCaptcha != null) txtCaptcha.Enabled = false;
            btnLogin.Enabled = false;
            btnGuest.Enabled = false;
            if (btnRefreshCaptcha != null) btnRefreshCaptcha.Enabled = false;

            blockTimer.Start();
        }

        private void EnableControls()
        {
            txtLogin.Enabled = true;
            txtPassword.Enabled = true;
            if (txtCaptcha != null) txtCaptcha.Enabled = true;
            btnLogin.Enabled = true;
            btnGuest.Enabled = true;
            if (btnRefreshCaptcha != null) btnRefreshCaptcha.Enabled = true;
        }

        // Закрытие формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (picCaptcha != null && picCaptcha.Image != null)
                picCaptcha.Image.Dispose();

            blockTimer?.Dispose();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}