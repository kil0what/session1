using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public class MainForm : BaseForm
    {
        private Button btnProducts;
        private Button btnOrders;
        private Button btnManageProducts;
        private Button btnCart;
        private Button btnLogout;
        private Label lblUserInfo;
        private Label lblWelcome;

        public MainForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = $"ООО Спортивные товары - {Session.CurrentUser?.Role ?? "Гость"}";

            // Информация о пользователе
            lblUserInfo = new Label
            {
                Text = $"👤 {Session.CurrentUser?.FullName ?? "Гость"}",
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true
            };

            // Приветствие
            lblWelcome = new Label
            {
                Text = $"Добро пожаловать, {Session.CurrentUser?.FullName ?? "Гость"}!",
                Font = new Font("Comic Sans MS", 14, FontStyle.Bold),
                ForeColor = accentColor,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(600, 40)
            };

            // Кнопки меню
            int centerX = (this.ClientSize.Width - 250) / 2;
            int buttonY = 150;
            int buttonWidth = 250;
            int buttonHeight = 50;
            int buttonSpacing = 15;

            // Товары (для всех)
            btnProducts = CreateMenuButton("📦 Просмотр товаров", centerX, buttonY, buttonWidth, buttonHeight);
            btnProducts.Click += BtnProducts_Click;
            buttonY += buttonHeight + buttonSpacing;

            // Заказы (для клиента и менеджера)
            if (Session.CurrentUser?.Role == "Клиент" || Session.CurrentUser?.Role == "Менеджер")
            {
                btnOrders = CreateMenuButton("📋 Мои заказы", centerX, buttonY, buttonWidth, buttonHeight);
                btnOrders.Click += BtnOrders_Click;
                buttonY += buttonHeight + buttonSpacing;
            }

            // Управление товарами (для администратора)
            if (Session.CurrentUser?.Role == "Администратор")
            {
                btnManageProducts = CreateMenuButton("⚙️ Управление товарами", centerX, buttonY, buttonWidth, buttonHeight);
                btnManageProducts.Click += BtnManageProducts_Click;
                buttonY += buttonHeight + buttonSpacing;
            }

            // Корзина (для всех кроме администратора)
            if (Session.CurrentUser?.Role != "Администратор")
            {
                btnCart = CreateMenuButton("🛒 Корзина", centerX, buttonY, buttonWidth, buttonHeight);
                btnCart.Click += BtnCart_Click;
                buttonY += buttonHeight + buttonSpacing;
            }

            // Кнопка выхода
            btnLogout = new Button
            {
                Text = "Выйти",
                Size = new Size(120, 40),
                Location = new Point(20, this.ClientSize.Height - 70),
                Font = new Font("Comic Sans MS", 10),
                BackColor = secondaryColor,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnLogout.Click += BtnLogout_Click;

            // Добавляем контролы
            this.Controls.AddRange(new Control[]
            {
                lblUserInfo, lblWelcome, btnProducts, btnOrders,
                btnManageProducts, btnCart, btnLogout
            }.Where(c => c != null).ToArray());
        }

        private Button CreateMenuButton(string text, int x, int y, int width, int height)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                Font = new Font("Comic Sans MS", 12, FontStyle.Bold),
                BackColor = accentColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Центрируем элементы
            int centerX = (this.ClientSize.Width - 250) / 2;

            if (btnProducts != null) btnProducts.Left = centerX;
            if (btnOrders != null) btnOrders.Left = centerX;
            if (btnManageProducts != null) btnManageProducts.Left = centerX;
            if (btnCart != null) btnCart.Left = centerX;

            if (lblWelcome != null)
            {
                lblWelcome.Left = (this.ClientSize.Width - lblWelcome.Width) / 2;
                lblWelcome.Top = 80;
            }

            if (lblUserInfo != null)
            {
                lblUserInfo.Location = new Point(20, 20);
            }

            if (btnLogout != null)
            {
                btnLogout.Top = this.ClientSize.Height - 70;
            }
        }

        private void BtnProducts_Click(object sender, EventArgs e)
        {
            ProductsForm productsForm = new ProductsForm();
            productsForm.ShowDialog();
        }

        private void BtnOrders_Click(object sender, EventArgs e)
        {
            // Форма заказов
            MessageBox.Show("Функционал заказов в разработке", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCart_Click(object sender, EventArgs e)
        {
            CartForm cartForm = new CartForm();
            cartForm.ShowDialog();
        }

        private void BtnManageProducts_Click(object sender, EventArgs e)
        {
            if (Session.CurrentUser?.Role == "Администратор")
            {
                ManageProductsForm manageForm = new ManageProductsForm();
                manageForm.ShowDialog();
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите выйти?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Session.CurrentUser = null;
                this.Close();
            }
        }
    }
}