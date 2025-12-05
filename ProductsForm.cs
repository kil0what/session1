using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public class ProductsForm : Form
    {
        private List<Product> allProducts;
        private List<Product> filteredProducts;

        // Элементы управления
        private TextBox txtSearch;
        private ComboBox cmbManufacturer;
        private ComboBox cmbCategory;
        private ComboBox cmbSort;
        private Label lblResults;
        private FlowLayoutPanel flowPanel;
        private Button btnReset;
        private Button btnViewCart;
        private Label lblCartCount;

        private PictureBox logoPictureBox;
        private Font mainFont = new Font("Comic Sans MS", 10);
        private Color backgroundColor = Color.White;
        private Color accentColor = Color.FromArgb(73, 140, 81);
        private Color secondaryColor = Color.FromArgb(118, 227, 131);

        public ProductsForm()
        {
            InitializeForm();
            LoadProducts();
        }

        private void InitializeForm()
        {
            this.Text = "ООО Спортивные товары - Каталог";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = backgroundColor;
            this.Font = mainFont;

            LoadLogo();
            CreateControls();
        }

        private void LoadLogo()
        {
            logoPictureBox = new PictureBox
            {
                Size = new Size(200, 60),
                Location = new Point(this.ClientSize.Width - 220, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            try
            {
                if (File.Exists("Resources/logo.png"))
                {
                    logoPictureBox.Image = Image.FromFile("Resources/logo.png");
                }
            }
            catch { }

            this.Controls.Add(logoPictureBox);
        }

        private void CreateControls()
        {
            // Поиск
            Label lblSearch = new Label
            {
                Text = "Поиск:",
                Location = new Point(20, 20),
                Size = new Size(60, 25)
            };

            txtSearch = new TextBox
            {
                Location = new Point(85, 20),
                Size = new Size(200, 25)
            };
            txtSearch.TextChanged += (s, e) => FilterProducts();

            // Производитель
            Label lblManufacturer = new Label
            {
                Text = "Производитель:",
                Location = new Point(300, 20),
                Size = new Size(110, 25)
            };

            cmbManufacturer = new ComboBox
            {
                Location = new Point(415, 20),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbManufacturer.SelectedIndexChanged += (s, e) => FilterProducts();

            // Категория
            Label lblCategory = new Label
            {
                Text = "Категория:",
                Location = new Point(580, 20),
                Size = new Size(80, 25)
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(665, 20),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.SelectedIndexChanged += (s, e) => FilterProducts();

            // Сортировка
            Label lblSort = new Label
            {
                Text = "Сортировка:",
                Location = new Point(830, 20),
                Size = new Size(90, 25)
            };

            cmbSort = new ComboBox
            {
                Location = new Point(925, 20),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSort.Items.AddRange(new string[] { "По умолчанию", "Цена ↑", "Цена ↓" });
            cmbSort.SelectedIndex = 0;
            cmbSort.SelectedIndexChanged += (s, e) => FilterProducts();

            // Результаты
            lblResults = new Label
            {
                Text = "Товары загружаются...",
                Location = new Point(20, 60),
                Size = new Size(300, 25),
                ForeColor = accentColor
            };

            // Корзина
            lblCartCount = new Label
            {
                Text = $"Корзина: {Cart.TotalItems} товаров",
                Location = new Point(700, 60),
                Size = new Size(150, 25),
                ForeColor = accentColor
            };

            btnViewCart = new Button
            {
                Text = "Перейти в корзину",
                Location = new Point(860, 55),
                Size = new Size(120, 30),
                BackColor = accentColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnViewCart.Click += (s, e) =>
            {
                CartForm cartForm = new CartForm();
                cartForm.ShowDialog();
                UpdateCartCount();
            };

            // Сброс
            btnReset = new Button
            {
                Text = "Сбросить",
                Location = new Point(1080, 20),
                Size = new Size(80, 25),
                BackColor = secondaryColor,
                FlatStyle = FlatStyle.Flat
            };
            btnReset.Click += (s, e) =>
            {
                txtSearch.Text = "";
                cmbManufacturer.SelectedIndex = 0;
                cmbCategory.SelectedIndex = 0;
                cmbSort.SelectedIndex = 0;
                FilterProducts();
            };

            // Панель товаров
            flowPanel = new FlowLayoutPanel
            {
                Location = new Point(20, 100),
                Size = new Size(960, 550),
                AutoScroll = true,
                BackColor = Color.White
            };

            this.Controls.AddRange(new Control[]
            {
                lblSearch, txtSearch,
                lblManufacturer, cmbManufacturer,
                lblCategory, cmbCategory,
                lblSort, cmbSort,
                btnReset,
                lblResults,
                lblCartCount, btnViewCart,
                flowPanel
            });
        }

        private void LoadProducts()
        {
            try
            {
                allProducts = DatabaseHelper.GetAllProducts();

                // Заполняем фильтры
                cmbManufacturer.Items.Add("Все производители");
                var manufacturers = allProducts
                    .Select(p => p.Manufacturer)
                    .Where(m => !string.IsNullOrEmpty(m))
                    .Distinct()
                    .OrderBy(m => m);
                foreach (var m in manufacturers)
                    cmbManufacturer.Items.Add(m);
                cmbManufacturer.SelectedIndex = 0;

                cmbCategory.Items.Add("Все категории");
                var categories = allProducts
                    .Select(p => p.Category)
                    .Where(c => !string.IsNullOrEmpty(c))
                    .Distinct()
                    .OrderBy(c => c);
                foreach (var c in categories)
                    cmbCategory.Items.Add(c);
                cmbCategory.SelectedIndex = 0;

                FilterProducts();
                UpdateCartCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterProducts()
        {
            if (allProducts == null) return;

            filteredProducts = allProducts.ToList();

            // Поиск
            string search = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                filteredProducts = filteredProducts.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    p.ArticleNumber.ToLower().Contains(search) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)))
                    .ToList();
            }

            // Производитель
            if (cmbManufacturer.SelectedIndex > 0)
            {
                string manufacturer = cmbManufacturer.SelectedItem.ToString();
                filteredProducts = filteredProducts
                    .Where(p => p.Manufacturer == manufacturer)
                    .ToList();
            }

            // Категория
            if (cmbCategory.SelectedIndex > 0)
            {
                string category = cmbCategory.SelectedItem.ToString();
                filteredProducts = filteredProducts
                    .Where(p => p.Category == category)
                    .ToList();
            }

            // Сортировка
            switch (cmbSort.SelectedItem?.ToString())
            {
                case "Цена ↑":
                    filteredProducts = filteredProducts.OrderBy(p => p.Price).ToList();
                    break;
                case "Цена ↓":
                    filteredProducts = filteredProducts.OrderByDescending(p => p.Price).ToList();
                    break;
            }

            // Обновляем результаты
            lblResults.Text = $"Найдено: {filteredProducts.Count} из {allProducts.Count}";

            // Отображаем товары
            DisplayProducts();
        }

        private void DisplayProducts()
        {
            flowPanel.Controls.Clear();

            foreach (var product in filteredProducts)
            {
                Panel card = CreateProductCard(product);
                flowPanel.Controls.Add(card);
            }
        }

        private Panel CreateProductCard(Product product)
        {
            Panel card = new Panel
            {
                Size = new Size(300, 400),
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = product.StockQuantity > 0 ? Color.White : Color.FromArgb(240, 240, 240)
            };

            // Изображение
            PictureBox pbImage = new PictureBox
            {
                Size = new Size(280, 180),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            // Загружаем изображение
            string imagePath = $"Resources/Images/{product.ImagePath}";
            if (File.Exists(imagePath))
            {
                pbImage.Image = Image.FromFile(imagePath);
            }
            else
            {
                pbImage.BackColor = Color.LightGray;
            }

            // Название
            Label lblName = new Label
            {
                Text = product.Name,
                Location = new Point(10, 200),
                Size = new Size(280, 40),
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold)
            };

            // Цена
            decimal finalPrice = product.Price * (1 - product.CurrentDiscount / 100);
            Label lblPrice = new Label
            {
                Text = product.CurrentDiscount > 0
                    ? $"{finalPrice:C} (скидка {product.CurrentDiscount}%)"
                    : $"{product.Price:C}",
                Location = new Point(10, 250),
                Size = new Size(280, 25),
                Font = new Font("Comic Sans MS", 11, FontStyle.Bold),
                ForeColor = accentColor
            };

            // Наличие
            Label lblStock = new Label
            {
                Text = product.StockQuantity > 0
                    ? $"В наличии: {product.StockQuantity} шт."
                    : "Нет в наличии",
                Location = new Point(10, 280),
                Size = new Size(280, 25),
                ForeColor = product.StockQuantity > 0 ? Color.Green : Color.Red
            };

            // Кнопка в корзину
            Button btnAdd = new Button
            {
                Text = "В корзину",
                Location = new Point(10, 320),
                Size = new Size(280, 40),
                BackColor = accentColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = product.StockQuantity > 0,
                Tag = product
            };
            btnAdd.Click += (s, e) =>
            {
                Cart.AddProduct(product);
                UpdateCartCount();
                MessageBox.Show($"Товар добавлен в корзину!\nВсего в корзине: {Cart.TotalItems} товаров",
                    "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            card.Controls.AddRange(new Control[] { pbImage, lblName, lblPrice, lblStock, btnAdd });
            return card;
        }

        private void UpdateCartCount()
        {
            lblCartCount.Text = $"Корзина: {Cart.TotalItems} товаров";
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (logoPictureBox != null)
            {
                logoPictureBox.Location = new Point(
                    this.ClientSize.Width - logoPictureBox.Width - 20,
                    10);
            }
        }
    }
}