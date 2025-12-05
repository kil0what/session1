using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public class OrdersForm : Form
    {
        private ListView listViewOrders;
        private Button btnRefresh;
        private Button btnDetails;
        private Button btnClose;
        private Button btnCreateOrder;
        private Label lblTitle;
        private Label lblEmpty;
        private PictureBox logoPictureBox;

        private List<Order> userOrders;

        public OrdersForm()
        {
            InitializeForm();
            LoadOrdersFromDatabase();
            LogoHelper.ApplyIcon(this)
            ;
        }

        private void InitializeForm()
        {
            this.Text = "ООО Спортивные товары - Мои заказы";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = LogoHelper.BackgroundColor;
            this.Font = LogoHelper.MainFont;
            this.MinimumSize = new Size(900, 550);

            // ЛОГОТИП (левый верхний угол)
            logoPictureBox = LogoHelper.CreateLogo();

            // Заголовок (рядом с логотипом)
            lblTitle = new Label
            {
                Text = $"📋 Мои заказы",
                Location = new Point(180, 15),
                Size = new Size(300, 40),
                Font = new Font("Comic Sans MS", 16, FontStyle.Bold),
                ForeColor = LogoHelper.AccentColor
            };

            // Список заказов
            listViewOrders = new ListView
            {
                View = View.Details,
                Location = new Point(20, 80),
                Size = new Size(1040, 400),
                FullRowSelect = true,
                GridLines = true,
                Font = LogoHelper.MainFont
            };

            listViewOrders.Columns.Add("№ Заказа", 100);
            listViewOrders.Columns.Add("Дата заказа", 120);
            listViewOrders.Columns.Add("Дата доставки", 120);
            listViewOrders.Columns.Add("Пункт выдачи", 300);
            listViewOrders.Columns.Add("Статус", 120);
            listViewOrders.Columns.Add("Сумма", 120);
            listViewOrders.Columns.Add("Код получения", 120);

            // Сообщение если нет заказов
            lblEmpty = new Label
            {
                Text = "У вас пока нет заказов\n\nПерейдите в каталог товаров, чтобы сделать первый заказ!",
                Location = new Point(300, 250),
                Size = new Size(500, 80),
                Font = new Font("Comic Sans MS", 12),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            // Кнопки
            btnRefresh = LogoHelper.CreateStyledButton(
                "🔄 Обновить",
                LogoHelper.SecondaryColor,
                Color.Black,
                140, 40);
            btnRefresh.Location = new Point(20, 500);
            btnRefresh.Click += (s, e) => LoadOrdersFromDatabase();

            btnDetails = LogoHelper.CreateStyledButton(
                "👁️ Детали заказа",
                LogoHelper.AccentColor,
                Color.White,
                160, 40);
            btnDetails.Location = new Point(170, 500);
            btnDetails.Enabled = false;
            btnDetails.Click += BtnDetails_Click;

            btnCreateOrder = LogoHelper.CreateStyledButton(
                "➕ Новый заказ",
                LogoHelper.AccentColor,
                Color.White,
                160, 40);
            btnCreateOrder.Location = new Point(340, 500);
            btnCreateOrder.Click += (s, e) =>
            {
                ProductsForm productsForm = new ProductsForm();
                productsForm.ShowDialog();
            };

            btnClose = LogoHelper.CreateStyledButton(
                "✕ Закрыть",
                Color.LightGray,
                Color.Black,
                120, 40);
            btnClose.Location = new Point(940, 500);
            btnClose.Click += (s, e) => this.Close();

            // Событие выбора заказа
            listViewOrders.SelectedIndexChanged += (s, e) =>
            {
                btnDetails.Enabled = listViewOrders.SelectedItems.Count > 0;
            };

            // Двойной клик для просмотра деталей
            listViewOrders.DoubleClick += (s, e) =>
            {
                if (listViewOrders.SelectedItems.Count > 0)
                {
                    BtnDetails_Click(null, null);
                }
            };

            this.Controls.AddRange(new Control[]
            {
                logoPictureBox, lblTitle, listViewOrders, lblEmpty,
                btnRefresh, btnDetails, btnCreateOrder, btnClose
            });
        }

        private void LoadOrdersFromDatabase()
        {
            listViewOrders.Items.Clear();

            try
            {
                // Получаем заказы из БД для текущего пользователя
                if (Session.CurrentUser == null ||
                    Session.CurrentUser.Role == "Guest" ||
                    Session.CurrentUser.Login == "guest")
                {
                    lblEmpty.Visible = true;
                    listViewOrders.Visible = false;
                    lblEmpty.Text = "Войдите в систему для просмотра заказов";
                    return;
                }

                userOrders = DatabaseHelper.GetUserOrders(Session.CurrentUser.FullName);

                if (userOrders.Count == 0)
                {
                    lblEmpty.Visible = true;
                    listViewOrders.Visible = false;
                    return;
                }

                lblEmpty.Visible = false;
                listViewOrders.Visible = true;

                foreach (var order in userOrders)
                {
                    decimal totalAmount = CalculateOrderTotal(order);

                    ListViewItem item = new ListViewItem(order.Id.ToString());
                    item.SubItems.Add(order.OrderDate.ToString("dd.MM.yyyy HH:mm"));
                    item.SubItems.Add(order.DeliveryDate.ToString("dd.MM.yyyy"));
                    item.SubItems.Add(DatabaseHelper.GetPickupPointAddress(order.PickupPointId));
                    item.SubItems.Add(order.Status);
                    item.SubItems.Add($"{totalAmount:C}");
                    item.SubItems.Add(order.Code);
                    item.Tag = order.Id;

                    // Цветовая индикация статуса
                    switch (order.Status)
                    {
                        case "Завершен":
                            item.BackColor = Color.FromArgb(232, 255, 234);
                            item.ForeColor = Color.Green;
                            break;
                        case "Новый":
                            item.BackColor = Color.FromArgb(255, 255, 200);
                            item.ForeColor = Color.DarkOrange;
                            break;
                        case "В обработке":
                            item.BackColor = Color.FromArgb(232, 240, 255);
                            item.ForeColor = Color.Blue;
                            break;
                        case "Отменен":
                            item.BackColor = Color.FromArgb(255, 232, 232);
                            item.ForeColor = Color.Red;
                            break;
                        default:
                            item.BackColor = Color.White;
                            break;
                    }

                    listViewOrders.Items.Add(item);
                }

                // Статистика
                int totalOrders = userOrders.Count;
                int completedOrders = userOrders.Count(o => o.Status == "Завершен");

                lblTitle.Text = $"📋 Мои заказы ({totalOrders} заказов, {completedOrders} завершено)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private decimal CalculateOrderTotal(Order order)
        {
            decimal total = 0;
            string[] items = order.OrderItems.Split(',');

            for (int i = 0; i < items.Length; i += 2)
            {
                if (i + 1 < items.Length)
                {
                    string article = items[i].Trim();
                    int quantity = int.Parse(items[i + 1].Trim());

                    // Получаем товар из БД
                    var product = DatabaseHelper.GetProductByArticle(article);
                    if (product != null)
                    {
                        decimal itemPrice = product.Price * (1 - product.CurrentDiscount / 100);
                        total += itemPrice * quantity;
                    }
                }
            }

            return total;
        }

        private void BtnDetails_Click(object sender, EventArgs e)
        {
            if (listViewOrders.SelectedItems.Count == 0) return;

            try
            {
                int orderId = (int)listViewOrders.SelectedItems[0].Tag;
                var order = userOrders.FirstOrDefault(o => o.Id == orderId);

                if (order != null)
                {
                    ShowOrderDetails(order);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия деталей заказа: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowOrderDetails(Order order)
        {
            try
            {
                string details = $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                               $"📋 ДЕТАЛИ ЗАКАЗА #{order.Id}\n" +
                               $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                               $"📅 Дата заказа: {order.OrderDate:dd.MM.yyyy HH:mm}\n" +
                               $"📦 Дата доставки: {order.DeliveryDate:dd.MM.yyyy}\n" +
                               $"📍 Пункт выдачи: {DatabaseHelper.GetPickupPointAddress(order.PickupPointId)}\n" +
                               $"🏷️ Статус: {order.Status}\n" +
                               $"🔢 Код получения: {order.Code}\n\n" +
                               $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                               $"🛒 СОСТАВ ЗАКАЗА:\n" +
                               $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                               $"{GetOrderItemsDetails(order)}";

                using (var detailForm = new OrderDetailsForm(details, order.Id))
                {
                    detailForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка формирования деталей заказа: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetOrderItemsDetails(Order order)
        {
            string result = "";
            string[] items = order.OrderItems.Split(',');
            decimal total = 0;

            for (int i = 0; i < items.Length; i += 2)
            {
                if (i + 1 < items.Length)
                {
                    string article = items[i].Trim();
                    int quantity = int.Parse(items[i + 1].Trim());

                    var product = DatabaseHelper.GetProductByArticle(article);

                    if (product != null)
                    {
                        decimal itemPrice = product.Price * (1 - product.CurrentDiscount / 100);
                        decimal itemTotal = itemPrice * quantity;
                        total += itemTotal;

                        result += $"▸ {product.Name}\n";
                        result += $"  Артикул: {article} | Кол-во: {quantity} шт.\n";
                        result += $"  Цена: {product.Price:C}";

                        if (product.CurrentDiscount > 0)
                        {
                            result += $" (скидка {product.CurrentDiscount}% = {itemPrice:C}/шт.)";
                        }

                        result += $"\n  Итого: {itemTotal:C}\n\n";
                    }
                    else
                    {
                        result += $"▸ Товар {article} (не найден в каталоге)\n";
                        result += $"  Кол-во: {quantity} шт.\n\n";
                    }
                }
            }

            result += $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n";
            result += $"💰 ОБЩАЯ СУММА: {total:C}\n";
            result += $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n";

            // Инструкция для получения заказа
            result += $"📋 ИНСТРУКЦИЯ ПОЛУЧЕНИЯ:\n";
            result += $"1. Подойдите к пункту выдачи\n";
            result += $"2. Назовите номер заказа: {order.Id}\n";
            result += $"3. Предъявите код: {order.Code}\n";
            result += $"4. Получите товар и проверьте комплектацию\n";

            return result;
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

            // Список заказов адаптируется
            if (listViewOrders != null)
            {
                listViewOrders.Width = this.ClientSize.Width - 40;
                listViewOrders.Height = this.ClientSize.Height - 250;
            }

            // Кнопки
            if (btnClose != null)
            {
                btnClose.Left = this.ClientSize.Width - btnClose.Width - 20;
                btnClose.Top = this.ClientSize.Height - btnClose.Height - 20;
            }

            if (btnDetails != null && btnRefresh != null && btnCreateOrder != null)
            {
                int bottomY = this.ClientSize.Height - 70;
                btnRefresh.Top = bottomY;
                btnDetails.Top = bottomY;
                btnCreateOrder.Top = bottomY;
            }

            // Центрируем сообщение о пустом списке
            if (lblEmpty != null)
            {
                lblEmpty.Left = (this.ClientSize.Width - lblEmpty.Width) / 2;
                lblEmpty.Top = (this.ClientSize.Height - lblEmpty.Height) / 2;
            }
        }
    }
}