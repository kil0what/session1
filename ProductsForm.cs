using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public partial class ProductsForm : Form
    {
        private List<Product> currentProducts;
        private List<Product> filteredProducts;
        private string currentSearch = "";
        private string currentManufacturer = "Все производители";
        private string currentCategory = "Все категории";
        private string currentSort = "По умолчанию";

        public ProductsForm()
        {
            InitializeComponent();
            LoadProducts();
            this.Load += ProductsForm_Load;
        }


        private void LoadProducts()
        {
            currentProducts = FakeDatabase.Products;

            // Заполняем список производителей
            var manufacturers = currentProducts
                .Select(p => p.Manufacturer)
                .Where(m => !string.IsNullOrEmpty(m))
                .Distinct()
                .OrderBy(m => m)
                .ToList();

            cmbManufacturer.Items.Clear();
            cmbManufacturer.Items.Add("Все производители");
            foreach (var manufacturer in manufacturers)
            {
                cmbManufacturer.Items.Add(manufacturer);
            }
            cmbManufacturer.SelectedIndex = 0;

            // Заполняем список категорий
            var categories = currentProducts
                .Select(p => p.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Все категории");
            foreach (var category in categories)
            {
                cmbCategory.Items.Add(category);
            }
            cmbCategory.SelectedIndex = 0;

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            filteredProducts = currentProducts.ToList();

            // Поиск
            if (!string.IsNullOrWhiteSpace(currentSearch) && currentSearch != "Название, артикул, описание...")
            {
                filteredProducts = filteredProducts.Where(p =>
                    (p.Name != null && p.Name.IndexOf(currentSearch, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (p.ArticleNumber != null && p.ArticleNumber.IndexOf(currentSearch, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (p.Description != null && p.Description.IndexOf(currentSearch, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (p.Category != null && p.Category.IndexOf(currentSearch, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (p.Manufacturer != null && p.Manufacturer.IndexOf(currentSearch, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();
            }

            // Фильтр по производителю
            if (currentManufacturer != "Все производители")
            {
                filteredProducts = filteredProducts
                    .Where(p => p.Manufacturer == currentManufacturer)
                    .ToList();
            }

            // Фильтр по категории
            if (currentCategory != "Все категории")
            {
                filteredProducts = filteredProducts
                    .Where(p => p.Category == currentCategory)
                    .ToList();
            }

            // Сортировка
            switch (currentSort)
            {
                case "Цена ↑":
                    filteredProducts = filteredProducts.OrderBy(p => p.Price).ToList();
                    break;
                case "Цена ↓":
                    filteredProducts = filteredProducts.OrderByDescending(p => p.Price).ToList();
                    break;
                case "Наличие ↓":
                    filteredProducts = filteredProducts.OrderByDescending(p => p.StockQuantity).ToList();
                    break;
            }

            // Обновляем статистику
            UpdateResultsLabel();
            DisplayProducts();
        }

        private void UpdateResultsLabel()
        {
            int total = currentProducts.Count;
            int shown = filteredProducts.Count;

            if (shown == total)
            {
                lblResults.Text = $"Показано: {shown} товаров";
            }
            else
            {
                lblResults.Text = $"Показано: {shown} из {total} товаров";
            }

            if (shown == 0)
            {
                lblResults.Text += " - товары не найдены";
                lblResults.ForeColor = Color.Red;
            }
            else
            {
                lblResults.ForeColor = Color.FromArgb(73, 140, 81); // Акцентный цвет
            }
        }

        private void DisplayProducts()
        {
            flowPanel.SuspendLayout();
            flowPanel.Controls.Clear();

            if (filteredProducts.Count == 0)
            {
                var lblNoResults = new Panel
                {
                    Size = new Size(flowPanel.ClientSize.Width - 40, 200),
                    BackColor = Color.FromArgb(250, 250, 250),
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10)
                };

                var icon = new Label
                {
                    Text = "😕",
                    Font = new Font("Arial", 48),
                    Location = new Point(20, 30),
                    Size = new Size(80, 80),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Black
                };

                var message = new Label
                {
                    Text = "Товары не найдены\nПопробуйте изменить критерии поиска",
                    Location = new Point(120, 40),
                    Size = new Size(400, 60),
                    Font = new Font("Comic Sans MS", 12),
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.Black
                };

                lblNoResults.Controls.AddRange(new Control[] { icon, message });
                flowPanel.Controls.Add(lblNoResults);
            }
            else
            {
                foreach (var product in filteredProducts)
                {
                    var productCard = CreateProductCard(product);
                    flowPanel.Controls.Add(productCard);
                }
            }

            flowPanel.ResumeLayout(true);
        }

        private Panel CreateProductCard(Product product)
        {
            Panel card = new Panel
            {
                Size = new Size(350, 450),
                Margin = new Padding(15),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = product.StockQuantity > 0 ? Color.White : Color.FromArgb(245, 245, 245),
                Cursor = Cursors.Hand,
                Tag = product
            };

            // События для подсветки карточки
            card.MouseEnter += (s, e) =>
            {
                card.BorderStyle = BorderStyle.FixedSingle;
                card.BackColor = Color.FromArgb(250, 250, 250);
            };

            card.MouseLeave += (s, e) =>
            {
                card.BorderStyle = BorderStyle.FixedSingle;
                card.BackColor = product.StockQuantity > 0 ? Color.White : Color.FromArgb(245, 245, 245);
            };

            // Изображение товара
            PictureBox pbImage = new PictureBox
            {
                Size = new Size(320, 200),
                Location = new Point(15, 15),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };

            // Загружаем изображение
            LoadProductImage(pbImage, product);

            // Название товара
            Label lblName = new Label
            {
                Text = product.Name ?? "Без названия",
                Location = new Point(15, 225),
                Size = new Size(320, 50),
                Font = new Font("Comic Sans MS", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.TopLeft,
                ForeColor = Color.Black
            };

            // Производитель и категория
            Label lblInfo = new Label
            {
                Text = $"{product.Manufacturer ?? "Не указан"} • {product.Category ?? "Без категории"}",
                Location = new Point(15, 280),
                Size = new Size(320, 20),
                Font = new Font("Comic Sans MS", 9),
                ForeColor = Color.Gray
            };

            // Цена
            decimal finalPrice = product.Price * (1 - product.CurrentDiscount / 100);
            Label lblPrice = new Label
            {
                Text = product.CurrentDiscount > 0
                    ? $"{finalPrice:C}  ↓{product.CurrentDiscount}%"
                    : $"{product.Price:C}",
                Location = new Point(15, 305),
                Size = new Size(320, 25),
                Font = new Font("Comic Sans MS", 12, FontStyle.Bold),
                ForeColor = product.CurrentDiscount > 0 ? Color.Red : Color.FromArgb(73, 140, 81) // Акцентный цвет
            };

            // Старая цена при скидке
            if (product.CurrentDiscount > 0)
            {
                Label lblOldPrice = new Label
                {
                    Text = $"{product.Price:C}",
                    Location = new Point(15, 330),
                    Size = new Size(150, 20),
                    Font = new Font("Comic Sans MS", 9, FontStyle.Strikeout),
                    ForeColor = Color.Gray
                };
                card.Controls.Add(lblOldPrice);
            }

            // Наличие
            Panel stockPanel = new Panel
            {
                Location = new Point(15, 360),
                Size = new Size(320, 30),
                BackColor = product.StockQuantity > 0 ? Color.FromArgb(232, 255, 234) : Color.FromArgb(255, 232, 232)
            };

            Label lblStock = new Label
            {
                Text = product.StockQuantity > 0
                    ? $"✅ В наличии: {product.StockQuantity} {product.Unit ?? "шт."}"
                    : "❌ Нет в наличии",
                Location = new Point(10, 5),
                Size = new Size(300, 20),
                Font = new Font("Comic Sans MS", 9, FontStyle.Bold),
                ForeColor = product.StockQuantity > 0 ? Color.Green : Color.Red,
                TextAlign = ContentAlignment.MiddleLeft
            };

            stockPanel.Controls.Add(lblStock);

            // Артикул
            Label lblArticle = new Label
            {
                Text = $"Арт: {product.ArticleNumber ?? "000000"}",
                Location = new Point(15, 400),
                Size = new Size(320, 20),
                Font = new Font("Comic Sans MS", 8),
                ForeColor = Color.DarkGray
            };

            // Кнопка быстрого добавления в корзину
            if (Session.IsClient || Session.IsGuest || Session.IsManager)
            {
                Button btnQuickAdd = new Button
                {
                    Text = "+",
                    Location = new Point(290, 395),
                    Size = new Size(40, 40),
                    BackColor = Color.FromArgb(73, 140, 81), // Акцентный цвет
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Comic Sans MS", 16, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    Tag = product
                };
                btnQuickAdd.Click += (s, e) => AddToCart(product);
                card.Controls.Add(btnQuickAdd);
            }

            card.Controls.AddRange(new Control[]
            {
                pbImage, lblName, lblInfo, lblPrice, stockPanel, lblArticle
            });

            return card;
        }

        private void LoadProductImage(PictureBox pictureBox, Product product)
        {
            try
            {
                // Пробуем разные пути к изображениям
                string[] possiblePaths =
                {
                    Path.Combine("Resources/Images/", product.ImagePath),
                    Path.Combine("Images/", product.ImagePath),
                    Path.Combine("Resources/", product.ImagePath),
                    product.ImagePath
                };

                foreach (var path in possiblePaths)
                {
                    if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    {
                        pictureBox.Image = Image.FromFile(path);
                        return;
                    }
                }

                // Если изображение не найдено, используем заглушку
                if (File.Exists("Resources/picture.png"))
                {
                    pictureBox.Image = Image.FromFile("Resources/picture.png");
                }
                else
                {
                    // Создаем простую заглушку
                    Bitmap bmp = new Bitmap(320, 200);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.White);
                        g.DrawRectangle(Pens.LightGray, 0, 0, 319, 199);
                        g.DrawString("Нет\nизображения",
                            new Font("Comic Sans MS", 16),
                            Brushes.Gray,
                            new RectangleF(0, 0, 320, 200),
                            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                    pictureBox.Image = bmp;
                }
            }
            catch
            {
                // В случае ошибки создаем простую заглушку
                pictureBox.BackColor = Color.LightGray;
                pictureBox.Image = null;
            }
        }

        private void AddToCart(Product product)
        {
            if (product.StockQuantity <= 0)
            {
                MessageBox.Show("Товар отсутствует на складе", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show($"Товар '{product.Name}' добавлен в корзину\nЦена: {product.Price:C}",
                "Добавлено в корзину", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Обработчики событий
        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Название, артикул, описание...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Название, артикул, описание...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text != "Название, артикул, описание...")
            {
                currentSearch = txtSearch.Text.Trim();
                ApplyFilters();
            }
        }

        private void CmbManufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbManufacturer.SelectedItem != null)
            {
                currentManufacturer = cmbManufacturer.SelectedItem.ToString();
                ApplyFilters();
            }
        }

        private void CmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedItem != null)
            {
                currentCategory = cmbCategory.SelectedItem.ToString();
                ApplyFilters();
            }
        }

        private void CmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSort.SelectedItem != null)
            {
                currentSort = cmbSort.SelectedItem.ToString();
                ApplyFilters();
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "Название, артикул, описание...";
            txtSearch.ForeColor = Color.Gray;
            cmbManufacturer.SelectedIndex = 0;
            cmbCategory.SelectedIndex = 0;
            cmbSort.SelectedIndex = 0;

            currentSearch = "";
            currentManufacturer = "Все производители";
            currentCategory = "Все категории";
            currentSort = "По умолчанию";

            ApplyFilters();
        }

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Выберите товар для добавления в корзину",
                "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ProductsForm_Load(object sender, EventArgs e)
        {
            // Центрируем элементы при загрузке
            if (lblResults != null)
            {
                lblResults.Left = (topPanel.Width - lblResults.Width) / 2;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Адаптация при изменении размера окна
            if (lblResults != null && topPanel != null)
            {
                lblResults.Left = (topPanel.Width - lblResults.Width) / 2;
            }


            if (flowPanel != null && filteredProducts != null)
            {
                DisplayProducts();
            }
        }
    }
}