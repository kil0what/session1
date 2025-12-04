using System;
using System.Drawing;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public class MainForm : Form
    {
        private Button btnProducts;
        private Button btnOrders;
        private Button btnManageProducts;
        private Button btnLogout;
        private Label lblUserInfo;

        public MainForm()
        {
            SetupMainForm();
        }

        private void SetupMainForm()
        {
            // Настройка формы
            this.Text = "ООО Спортивные товары - Главная";
            this.Font = new Font("Comic Sans MS", 10);
            this.BackColor = Color.White;
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 600);

            // Заголовок с логотипом
            Panel headerPanel = new Panel
            {
                BackColor = Color.FromArgb(118, 227, 131),
                Height = 80,
                Dock = DockStyle.Top
            };
            this.Controls.Add(headerPanel);

            // Информация о пользователе (правый верхний угол)
            lblUserInfo = new Label
            {
                Text = Session.CurrentUser?.FullName ?? "Гость",
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false, // Изменяем на false для лучшего контроля
                Size = new Size(200, 30)
            };

            // Добавляем на панель
            headerPanel.Controls.Add(lblUserInfo);

            // Обновляем позицию после добавления
            this.Load += (s, e) =>
            {
                lblUserInfo.Location = new Point(
                    headerPanel.Width - lblUserInfo.Width - 20,
                    (headerPanel.Height - lblUserInfo.Height) / 2);
            };

            // Кнопки меню - делаем центрированными
            int centerX = (this.ClientSize.Width - 250) / 2;
            int buttonY = 150;
            int buttonWidth = 250;
            int buttonHeight = 50;
            int buttonSpacing = 20;

            // Товары (для всех)
            btnProducts = new Button
            {
                Text = "📦 Просмотр товаров",
                Location = new Point(centerX, buttonY),
                Size = new Size(buttonWidth, buttonHeight),
                BackColor = Color.FromArgb(73, 140, 81),
                ForeColor = Color.White,
                Font = new Font("Comic Sans MS", 12, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnProducts.Click += BtnProducts_Click;
            this.Controls.Add(btnProducts);

            buttonY += buttonHeight + buttonSpacing;

            // Заказы (только для клиента и менеджера)
            if (Session.CurrentUser?.Role == "Client" || Session.CurrentUser?.Role == "Manager")
            {
                btnOrders = new Button
                {
                    Text = "🛒 Мои заказы",
                    Location = new Point(centerX, buttonY),
                    Size = new Size(buttonWidth, buttonHeight),
                    BackColor = Color.FromArgb(73, 140, 81),
                    ForeColor = Color.White,
                    Font = new Font("Comic Sans MS", 12, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnOrders.Click += BtnOrders_Click;
                this.Controls.Add(btnOrders);
                buttonY += buttonHeight + buttonSpacing;
            }

            // Управление товарами (только для администратора)
            if (Session.CurrentUser?.Role == "Admin")
            {
                btnManageProducts = new Button
                {
                    Text = "⚙️ Управление товарами",
                    Location = new Point(centerX, buttonY),
                    Size = new Size(buttonWidth, buttonHeight),
                    BackColor = Color.FromArgb(73, 140, 81),
                    ForeColor = Color.White,
                    Font = new Font("Comic Sans MS", 12, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnManageProducts.Click += BtnManageProducts_Click;
                this.Controls.Add(btnManageProducts);
                buttonY += buttonHeight + buttonSpacing;
            }

            // Кнопка выхода (внизу слева)
            btnLogout = new Button
            {
                Text = "Выйти",
                Location = new Point(20, this.ClientSize.Height - 70),
                Size = new Size(120, 40),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                Font = new Font("Comic Sans MS", 10),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogout.Click += BtnLogout_Click;
            this.Controls.Add(btnLogout);
        }

        private void BtnProducts_Click(object sender, EventArgs e)
        {
            ProductsForm productsForm = new ProductsForm();
            productsForm.ShowDialog();
        }

        private void BtnOrders_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Форма заказов в разработке", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnManageProducts_Click(object sender, EventArgs e)
        {
            if (Session.CurrentUser?.Role == "Admin")
            {
                ManageProductsForm manageForm = new ManageProductsForm();
                manageForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Доступ запрещен", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы действительно хотите выйти?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Session.CurrentUser = null;

                // Возвращаемся к окну входа
                Form1 loginForm = new Form1();
                loginForm.Show();
                this.Close();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (btnLogout != null)
            {
                btnLogout.Location = new Point(20, this.ClientSize.Height - 70);
            }

            // Центрируем кнопки при изменении размера окна
            if (btnProducts != null)
            {
                int centerX = (this.ClientSize.Width - btnProducts.Width) / 2;
                btnProducts.Location = new Point(centerX, btnProducts.Location.Y);

                if (btnOrders != null)
                    btnOrders.Location = new Point(centerX, btnOrders.Location.Y);

                if (btnManageProducts != null)
                    btnManageProducts.Location = new Point(centerX, btnManageProducts.Location.Y);
            }
        }
    }
}