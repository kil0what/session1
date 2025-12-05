using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public class CartForm : Form
    {
        private ListView listViewCart;
        private Label lblTotal;
        private Button btnClear;
        private Button btnCheckout;
        private Button btnClose;
        private PictureBox logoPictureBox;
        private Label lblTitle;

        public CartForm()
        {
            InitializeForm();
            LoadCart();
            LogoHelper.ApplyIcon(this);
        }

        private void InitializeForm()
        {
            this.Text = "ООО Спортивные товары - Корзина";
            this.Size = new Size(700, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = LogoHelper.BackgroundColor;
            this.Font = LogoHelper.MainFont;
            this.MinimumSize = new Size(600, 450);

            // ЛОГОТИП (левый верхний угол)
            logoPictureBox = LogoHelper.CreateLogo();

            // Заголовок (рядом с логотипом)
            lblTitle = new Label
            {
                Text = "🛒 Корзина покупок",
                Location = new Point(180, 15),
                Size = new Size(300, 40),
                Font = new Font("Comic Sans MS", 16, FontStyle.Bold),
                ForeColor = LogoHelper.AccentColor
            };

            // Список товаров
            listViewCart = new ListView
            {
                View = View.Details,
                Location = new Point(20, 80),
                Size = new Size(650, 300),
                GridLines = true,
                FullRowSelect = true,
                Font = LogoHelper.MainFont
            };

            listViewCart.Columns.Add("Товар", 300);
            listViewCart.Columns.Add("Цена", 100);
            listViewCart.Columns.Add("Кол-во", 80);
            listViewCart.Columns.Add("Сумма", 100);

            // Итого
            lblTotal = new Label
            {
                Text = $"Итого: {Cart.TotalAmount:C}",
                Location = new Point(20, 400),
                Size = new Size(300, 30),
                Font = new Font("Comic Sans MS", 12, FontStyle.Bold),
                ForeColor = LogoHelper.AccentColor
            };

            // Кнопки
            btnClear = LogoHelper.CreateStyledButton(
                "🗑️ Очистить корзину",
                Color.LightGray,
                Color.Black,
                150, 40);
            btnClear.Location = new Point(20, 450);
            btnClear.Click += (s, e) =>
            {
                if (Cart.Items.Count == 0)
                {
                    MessageBox.Show("Корзина уже пуста", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show("Очистить всю корзину?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Cart.Clear();
                    LoadCart();
                }
            };

            btnCheckout = LogoHelper.CreateStyledButton(
                "💳 Оформить заказ",
                LogoHelper.AccentColor,
                Color.White,
                150, 40);
            btnCheckout.Location = new Point(190, 450);
            btnCheckout.Click += (s, e) =>
            {
                if (Cart.Items.Count == 0)
                {
                    MessageBox.Show("Корзина пуста", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show($"Заказ оформлен на сумму {Cart.TotalAmount:C}\nТоваров: {Cart.TotalItems}",
                    "Заказ оформлен", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Cart.Clear();
                LoadCart();
            };

            btnClose = LogoHelper.CreateStyledButton(
                "Закрыть",
                LogoHelper.SecondaryColor,
                Color.Black,
                120, 40);
            btnClose.Location = new Point(550, 450);
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[]
            {
                logoPictureBox, lblTitle, listViewCart, lblTotal,
                btnClear, btnCheckout, btnClose
            });
        }

        private void LoadCart()
        {
            listViewCart.Items.Clear();

            foreach (var item in Cart.Items)
            {
                decimal finalPrice = item.Product.Price * (1 - item.Product.CurrentDiscount / 100);
                decimal total = finalPrice * item.Quantity;

                var listItem = new ListViewItem(item.Product.Name);
                listItem.SubItems.Add($"{item.Product.Price:C}");
                listItem.SubItems.Add(item.Quantity.ToString());
                listItem.SubItems.Add($"{total:C}");
                listViewCart.Items.Add(listItem);
            }

            lblTotal.Text = $"Итого: {Cart.TotalAmount:C}";

            // Если корзина пуста
            if (Cart.Items.Count == 0)
            {
                lblTotal.Text = "Корзина пуста";
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Логотип всегда в левом верхнем углу
            if (logoPictureBox != null)
            {
                logoPictureBox.Location = new Point(20, 10);
            }

            // Заголовок рядом с логотипом
            if (lblTitle != null)
            {
                lblTitle.Location = new Point(180, 15);
            }

            // Обновляем размеры списка
            if (listViewCart != null)
            {
                listViewCart.Width = this.ClientSize.Width - 40;
                listViewCart.Height = this.ClientSize.Height - 250;
            }

            // Позиционируем кнопки
            if (btnClose != null)
            {
                btnClose.Left = this.ClientSize.Width - btnClose.Width - 20;
                btnClose.Top = this.ClientSize.Height - btnClose.Height - 20;
            }

            if (btnCheckout != null)
            {
                btnCheckout.Top = this.ClientSize.Height - btnCheckout.Height - 20;
            }

            if (btnClear != null)
            {
                btnClear.Top = this.ClientSize.Height - btnClear.Height - 20;
            }

            // Итого
            if (lblTotal != null)
            {
                lblTotal.Top = this.ClientSize.Height - 130;
            }
        }
    }
}