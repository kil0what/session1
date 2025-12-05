using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public class Form1 : Form
    {
        // Элементы управления
        private TextBox txtLogin;
        private TextBox txtPassword;
        private TextBox txtCaptcha;
        private PictureBox picCaptcha;
        private Button btnLogin;
        private Button btnGuest;
        private Button btnRefreshCaptcha;
        private Label lblBlockTimer;
        private Label lblLogin;
        private Label lblPassword;
        private PictureBox logoPictureBox;

        private Timer blockTimer;
        private string generatedCaptcha;
        private bool captchaRequired = false;
        private int blockSecondsRemaining = 0;
        private int failedAttempts = 0;
        private DateTime? blockedUntil = null;

        public Form1()
        {
            InitializeComponents();
            SetupEventHandlers();
            LogoHelper.ApplyIcon(this);
            ApplyStyles();
        }

        private void InitializeComponents()
        {
            this.Text = "ООО Спортивные товары - Вход";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            LogoHelper.ApplyDefaultStyles(this);

            // ЛОГОТИП (левый верхний угол)
            logoPictureBox = LogoHelper.CreateLogo();

            // Заголовок (смещен вправо от логотипа)
            Label lblTitle = new Label
            {
                Text = "Вход в систему",
                Location = new Point(180, 20),
                Size = new Size(200, 40),
                Font = new Font("Comic Sans MS", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = LogoHelper.AccentColor
            };

            // Логин (ниже логотипа)
            lblLogin = new Label
            {
                Text = "Логин:",
                Location = new Point(100, 80),
                Size = new Size(100, 25),
                Font = LogoHelper.MainFont
            };

            txtLogin = new TextBox
            {
                Location = new Point(200, 80),
                Size = new Size(200, 25),
                Font = LogoHelper.MainFont
            };

            // Пароль
            lblPassword = new Label
            {
                Text = "Пароль:",
                Location = new Point(100, 120),
                Size = new Size(100, 25),
                Font = LogoHelper.MainFont
            };

            txtPassword = new TextBox
            {
                Location = new Point(200, 120),
                Size = new Size(200, 25),
                Font = LogoHelper.MainFont,
                PasswordChar = '*'
            };

            // CAPTCHA
            picCaptcha = new PictureBox
            {
                Location = new Point(150, 170),
                Size = new Size(150, 50),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };

            txtCaptcha = new TextBox
            {
                Location = new Point(310, 170),
                Size = new Size(90, 25),
                Font = LogoHelper.MainFont,
                Visible = false
            };

            btnRefreshCaptcha = new Button
            {
                Text = "🔄",
                Location = new Point(310, 200),
                Size = new Size(90, 25),
                Font = LogoHelper.MainFont,
                Visible = false
            };

            // Кнопки
            btnLogin = LogoHelper.CreateStyledButton(
                "Войти",
                LogoHelper.AccentColor,
                Color.White,
                120, 40);
            btnLogin.Location = new Point(100, 250);

            btnGuest = LogoHelper.CreateStyledButton(
                "Войти как гость",
                LogoHelper.SecondaryColor,
                Color.Black,
                120, 40);
            btnGuest.Location = new Point(250, 250);

            // Блокировка
            lblBlockTimer = new Label
            {
                Location = new Point(100, 310),
                Size = new Size(300, 30),
                Font = LogoHelper.MainFont,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Red,
                Visible = false
            };

            // Таймер
            blockTimer = new Timer();
            blockTimer.Interval = 1000;

            // Добавляем все элементы на форму
            this.Controls.AddRange(new Control[]
            {
                logoPictureBox, lblTitle, lblLogin, txtLogin,
                lblPassword, txtPassword, picCaptcha, txtCaptcha,
                btnRefreshCaptcha, btnLogin, btnGuest, lblBlockTimer
            });
        }

        private void SetupEventHandlers()
        {
            // ВАЖНО: Добавляем обработчики кликов
            btnLogin.Click += BtnLogin_Click;
            btnGuest.Click += BtnGuest_Click;
            btnRefreshCaptcha.Click += BtnRefreshCaptcha_Click;
            blockTimer.Tick += BlockTimer_Tick;
        }

        private void ApplyStyles()
        {
            // Применяем стили из ТЗ
            btnLogin.BackColor = Color.FromArgb(73, 140, 81); // Акцентный цвет
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Comic Sans MS", 10, FontStyle.Bold);

            btnGuest.BackColor = Color.FromArgb(118, 227, 131); // Дополнительный цвет
            btnGuest.ForeColor = Color.Black;
            btnGuest.FlatStyle = FlatStyle.Flat;
            btnGuest.Font = new Font("Comic Sans MS", 10);
        }

        // ========== ОБРАБОТЧИКИ СОБЫТИЙ ==========

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;
            string captcha = captchaRequired ? txtCaptcha.Text.Trim() : null;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка",
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

            // Здесь должна быть проверка в базе данных
            bool success = CheckLogin(login, password);

            if (success)
            {
                MessageBox.Show("Вход успешен!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Открываем главную форму
                OpenMainWindow();
            }
            else
            {
                failedAttempts++;
                MessageBox.Show($"Неверный логин или пароль. Попытка {failedAttempts}/2",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckLogin(string login, string password)
        {
            try
            {
                // Проверяем подключение к БД
                if (!DatabaseHelper.TestConnection())
                {
                    MessageBox.Show("Нет подключения к БД", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Пробуем авторизоваться
                var user = DatabaseHelper.AuthenticateUser(login, password);

                if (user != null)
                {
                    Session.CurrentUser = user;
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void BtnGuest_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Вход как гость", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            Session.CurrentUser = new User
            {
                Id = 0,
                Login = "guest",
                Role = "Guest",
                FullName = "Гость"
            };

            OpenMainWindow();
        }

        private void BtnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            GenerateCaptcha();
        }

        private void BlockTimer_Tick(object sender, EventArgs e)
        {
            // Код таймера блокировки
        }

        private void GenerateCaptcha()
        {
            // Генерация CAPTCHA
        }

        private void OpenMainWindow()
        {
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.FormClosed += (s, args) => this.Close();
            mainForm.Show();
        }

        // ========== ТЕСТОВАЯ КНОПКА для проверки ==========
        // Добавьте эту кнопку временно для отладки:
        private void AddTestButton()
        {
            Button testBtn = new Button
            {
                Text = "Тест",
                Location = new Point(100, 400),
                Size = new Size(100, 30),
                BackColor = Color.Red
            };
            testBtn.Click += (s, e) => MessageBox.Show("Тест работает!");
            this.Controls.Add(testBtn);
        }
    }
}