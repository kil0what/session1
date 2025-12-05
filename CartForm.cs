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

        public CartForm()
        {
            InitializeForm();
            LoadCart();
        }

        private void InitializeForm()
        {
            this.Text = "Корзина";
            this.Size = new Size(600, 500);
            this.BackColor = Color.White;
            this.Font = new Font("Comic Sans MS", 10);

            // Список
            listViewCart = new ListView
            {
                View = View.Details,
                Location = new Point(20, 20),
                Size = new Size(540, 350),
                GridLines = true,
                FullRowSelect = true
            };

            listViewCart.Columns.Add("Товар", 250);
            listViewCart.Columns.Add("Цена", 100);
            listViewCart.Columns.Add("Кол-во", 80);
            listViewCart.Columns.Add("Сумма", 100);

            // Итого
            lblTotal = new Label
            {
                Text = $"Итого: {Cart.TotalAmount:C}",
                Location = new Point(20, 380),
                Size = new Size(300, 30),
                Font = new Font("Comic Sans MS", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(73, 140, 81)
            };

            // Кнопки
            btnClear = new Button
            {
                Text = "Очистить",
                Location = new Point(20, 420),
                Size = new Size(100, 40),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            btnClear.Click += (s, e) =>
            {
                Cart.Clear();
                LoadCart();
            };

            btnCheckout = new Button
            {
                Text = "Оформить",
                Location = new Point(140, 420),
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(73, 140, 81),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCheckout.Click += (s, e) =>
            {
                MessageBox.Show("Заказ оформлен!", "Успешно",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cart.Clear();
                this.Close();
            };

            btnClose = new Button
            {
                Text = "Закрыть",
                Location = new Point(460, 420),
                Size = new Size(100, 40),
                BackColor = Color.FromArgb(118, 227, 131),
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[]
            {
                listViewCart, lblTotal, btnClear, btnCheckout, btnClose
            });
        }

        private void LoadCart()
        {
            listViewCart.Items.Clear();

            foreach (var item in Cart.Items)
            {
                var listItem = new ListViewItem(item.Product.Name);
                listItem.SubItems.Add($"{item.Product.Price:C}");
                listItem.SubItems.Add(item.Quantity.ToString());
                listItem.SubItems.Add($"{item.TotalPrice:C}");
                listViewCart.Items.Add(listItem);
            }

            lblTotal.Text = $"Итого: {Cart.TotalAmount:C}";
        }
    }
}