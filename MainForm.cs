using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public class MainForm : Form
    {
        private Button btnProducts;
        private Button btnOrders;
        private Button btnManageProducts;
        private Button btnCart;
        private Button btnLogout;
        private Label lblUserInfo;
        private Label lblWelcome;
        private PictureBox logoPictureBox;

        public MainForm()
        {
            InitializeForm();
            LogoHelper.ApplyIcon(this)
            ;
        }

        private void InitializeForm()
        {
            this.Text = $"ООО Спортивные товары - {Session.CurrentUser?.Role ?? "Гость"}";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = LogoHelper.BackgroundColor;
            this.Font = LogoHelper.MainFont;
            this.MinimumSize = new Size(800, 500);

            // ЛОГОТИП (левый верхний угол)
            logoPictureBox = LogoHelper.CreateLogo();

            // Информация о пользователе (правый верхний угол)
            lblUserInfo = new Label
            {
                Text = $"👤 {Session.CurrentUser?.FullName ?? "Гость"}",
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(this.ClientSize.Width - 250, 20)
            };

            // Приветствие (под логотипом)
            lblWelcome = new Label
            {
                Text = $"Добро пожаловать, {Session.CurrentUser?.FullName ?? "Гость"}!",
                Font = new Font("Comic Sans MS", 14, FontStyle.Bold),
                ForeColor = LogoHelper.AccentColor,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(600, 40),
                Location = new Point(150, 80)
            };

            // Кнопки меню (ниже приветствия)
            int centerX = (this.ClientSize.Width - 300) / 2;
            int buttonY = 150;
            int buttonWidth = 300;
            int buttonHeight = 50;
            int buttonSpacing = 10;

            // Товары (для всех)
            btnProducts = LogoHelper.CreateStyledButton(
                "📦 Просмотр товаров",
                LogoHelper.AccentColor,
                Color.White,
                buttonWidth, buttonHeight);
            btnProducts.Location = new Point(centerX, buttonY);
            btnProducts.Click += (s, e) =>
            {
                ProductsForm productsForm = new ProductsForm();
                productsForm.ShowDialog();
            };
            buttonY += buttonHeight + buttonSpacing;

            // Заказы (только для клиента и менеджера)
            if (Session.CurrentUser?.Role == "Клиент" || Session.CurrentUser?.Role == "Менеджер")
            {
                btnOrders = LogoHelper.CreateStyledButton(
                    "📋 Мои заказы",
                    LogoHelper.AccentColor,
                    Color.White,
                    buttonWidth, buttonHeight);
                btnOrders.Location = new Point(centerX, buttonY);
                btnOrders.Click += (s, e) =>
                {
                    OrdersForm ordersForm = new OrdersForm();
                    ordersForm.ShowDialog();
                };
                buttonY += buttonHeight + buttonSpacing;
            }

            // Управление товарами (только для администратора)
            if (Session.CurrentUser?.Role == "Администратор")
            {
                btnManageProducts = LogoHelper.CreateStyledButton(
                    "⚙️ Управление товарами",
                    LogoHelper.AccentColor,
                    Color.White,
                    buttonWidth, buttonHeight);
                btnManageProducts.Location = new Point(centerX, buttonY);
                btnManageProducts.Click += (s, e) =>
                {
                    ManageProductsForm manageForm = new ManageProductsForm();
                    manageForm.ShowDialog();
                };
                buttonY += buttonHeight + buttonSpacing;
            }

            // Корзина (для всех кроме администратора)
            if (Session.CurrentUser?.Role != "Администратор")
            {
                btnCart = LogoHelper.CreateStyledButton(
                    "🛒 Корзина",
                    LogoHelper.AccentColor,
                    Color.White,
                    buttonWidth, buttonHeight);
                btnCart.Location = new Point(centerX, buttonY);
                btnCart.Click += (s, e) =>
                {
                    CartForm cartForm = new CartForm();
                    cartForm.ShowDialog();
                };
                buttonY += buttonHeight + buttonSpacing;
            }

            // Кнопка выхода (нижний правый угол)
            btnLogout = LogoHelper.CreateStyledButton(
                "Выйти",
                LogoHelper.SecondaryColor,
                Color.Black,
                120, 40);
            btnLogout.Location = new Point(this.ClientSize.Width - 140, this.ClientSize.Height - 60);
            btnLogout.Click += (s, e) =>
            {
                DialogResult result = MessageBox.Show("Вы действительно хотите выйти?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Session.CurrentUser = null;
                    this.Close();
                }
            };

            // Добавляем все контролы
            this.Controls.AddRange(new Control[]
            {
                logoPictureBox, lblUserInfo, lblWelcome, btnProducts, btnOrders,
                btnManageProducts, btnCart, btnLogout
            }.Where(c => c != null).ToArray());
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Логотип всегда в левом верхнем углу
            if (logoPictureBox != null)
            {
                logoPictureBox.Location = new Point(20, 10);
            }

            // Центрируем элементы при изменении размера окна
            int centerX = (this.ClientSize.Width - 300) / 2;

            if (btnProducts != null)
            {
                btnProducts.Left = centerX;
                btnProducts.Top = 150;
            }

            if (btnOrders != null)
            {
                btnOrders.Left = centerX;
                btnOrders.Top = btnProducts != null ? btnProducts.Bottom + 10 : 150;
            }

            if (btnManageProducts != null)
            {
                btnManageProducts.Left = centerX;
                btnManageProducts.Top = btnOrders != null ? btnOrders.Bottom + 10 : btnProducts.Bottom + 10;
            }

            if (btnCart != null)
            {
                btnCart.Left = centerX;
                btnCart.Top = btnManageProducts != null ? btnManageProducts.Bottom + 10 :
                             btnOrders != null ? btnOrders.Bottom + 10 : btnProducts.Bottom + 10;
            }

            // Позиционируем приветствие
            if (lblWelcome != null)
            {
                lblWelcome.Left = (this.ClientSize.Width - lblWelcome.Width) / 2;
                lblWelcome.Top = 80;
            }

            // Позиционируем информацию о пользователе (правый верхний угол)
            if (lblUserInfo != null)
            {
                lblUserInfo.Left = this.ClientSize.Width - lblUserInfo.Width - 20;
                lblUserInfo.Top = 20;
            }

            // Позиционируем кнопку выхода (правый нижний угол)
            if (btnLogout != null)
            {
                btnLogout.Left = this.ClientSize.Width - btnLogout.Width - 20;
                btnLogout.Top = this.ClientSize.Height - btnLogout.Height - 20;
            }
        }
    }
}