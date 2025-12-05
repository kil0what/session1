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

        public ProductsForm()
        {
            InitializeForm();
            LoadProducts();
            LogoHelper.ApplyIcon(this)
            ;
        }

        private void InitializeForm()
        {
            this.Text = "ООО Спортивные товары - Каталог";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = LogoHelper.BackgroundColor;
            this.Font = LogoHelper.MainFont;
            this.MinimumSize = new Size(1000, 650);

            // ЛОГОТИП (левый верхний угол)
            logoPictureBox = LogoHelper.CreateLogo();

            // Панель поиска (смещена вниз от логотипа)
            Panel searchPanel = new Panel
            {
                Location = new Point(0, 70), // Ниже логотипа
                Size = new Size(this.ClientSize.Width, 100),
                BackColor = LogoHelper.SecondaryColor
            };

            // Поиск
            Label lblSearch = new Label
            {
                Text = "Поиск:",
                Location = new Point(20, 15),
                Size = new Size(60, 25),
                ForeColor = Color.Black,
                Font = LogoHelper.MainFont
            };

            txtSearch = new TextBox
            {
                Location = new Point(85, 15),
                Size = new Size(200, 25),
                Font = LogoHelper.MainFont
            };
            txtSearch.TextChanged += (s, e) => FilterProducts();

            // Производитель
            Label lblManufacturer = new Label
            {
                Text = "Производитель:",
                Location = new Point(300, 15),
                Size = new Size(110, 25),
                ForeColor = Color.Black,
                Font = LogoHelper.MainFont
            };

            cmbManufacturer = new ComboBox
            {
                Location = new Point(415, 15),
                Size = new Size(160, 25),
                Font = LogoHelper.MainFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbManufacturer.SelectedIndexChanged += (s, e) => FilterProducts();

            // Категория
            Label lblCategory = new Label
            {
                Text = "Категория:",
                Location = new Point(590, 15),
                Size = new Size(80, 25),
                ForeColor = Color.Black,
                Font = LogoHelper.MainFont
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(675, 15),
                Size = new Size(160, 25),
                Font = LogoHelper.MainFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.SelectedIndexChanged += (s, e) => FilterProducts();

            // Сортировка
            Label lblSort = new Label
            {
                Text = "Сортировка:",
                Location = new Point(850, 15),
                Size = new Size(90, 25),
                ForeColor = Color.Black,
                Font = LogoHelper.MainFont
            };

            cmbSort = new ComboBox
            {
                Location = new Point(945, 15),
                Size = new Size(160, 25),
                Font = LogoHelper.MainFont,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbSort.Items.AddRange(new string[] { "По умолчанию", "Цена ↑", "Цена ↓", "Наличие ↓" });
            cmbSort.SelectedIndex = 0;
            cmbSort.SelectedIndexChanged += (s, e) => FilterProducts();

            // Кнопка сброса
            btnReset = LogoHelper.CreateStyledButton(
                "Сбросить фильтры",
                LogoHelper.AccentColor,
                Color.White,
                120, 25, new Font("Comic Sans MS", 9));
            btnReset.Location = new Point(1120, 15);
            btnReset.Click += (s, e) =>
            {
                txtSearch.Text = "";
                cmbManufacturer.SelectedIndex = 0;
                cmbCategory.SelectedIndex = 0;
                cmbSort.SelectedIndex = 0;
                FilterProducts();
            };

            // Результаты поиска
            lblResults = new Label
            {
                Text = "Загрузка...",
                Location = new Point(20, 50),
                Size = new Size(400, 25),
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold),
                ForeColor = LogoHelper.AccentColor
            };

            // Корзина
            lblCartCount = new Label
            {
                Text = $"🛒 Корзина: {Cart.TotalItems} товаров",
                Location = new Point(850, 50),
                Size = new Size(200, 25),
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold),
                ForeColor = LogoHelper.AccentColor
            };

            // Кнопка корзины
            btnViewCart = LogoHelper.CreateStyledButton(
                "Перейти в корзину",
                LogoHelper.AccentColor,
                Color.White,
                140, 25, new Font("Comic Sans MS", 9));
            btnViewCart.Location = new Point(1050, 50);
            btnViewCart.Click += (s, e) =>
            {
                CartForm cartForm = new CartForm();
                cartForm.ShowDialog();
                UpdateCartCount();
            };

            // Добавляем элементы на панель поиска
            searchPanel.Controls.AddRange(new Control[]
            {
                lblSearch, txtSearch,
                lblManufacturer, cmbManufacturer,
                lblCategory, cmbCategory,
                lblSort, cmbSort,
                btnReset,
                lblResults,
                lblCartCount, btnViewCart
            });

            // Панель товаров (ниже панели поиска)
            flowPanel = new FlowLayoutPanel
            {
                Location = new Point(20, 180), // Ниже всех панелей
                Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 200),
                AutoScroll = true,
                BackColor = Color.White,
                WrapContents = true,
                AutoSize = false
            };

            // Добавляем все на форму
            this.Controls.AddRange(new Control[] { logoPictureBox, searchPanel, flowPanel });
        }

        public void RefreshProducts()
        {
            LoadProducts(); // Перезагружаем товары из БД
        }

        private void LoadProducts()
        {
            try
            {
                allProducts = DatabaseHelper.GetAllProducts();

                if (allProducts.Count == 0)
                {
                    MessageBox.Show("В базе данных нет товаров.",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Заполняем фильтры
                LoadFilters();

                FilterProducts();
                UpdateCartCount();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFilters()
        {
            // Производители
            var manufacturers = allProducts
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

            // Категории
            var categories = allProducts
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
        }

        private void FilterProducts()
        {
            if (allProducts == null) return;

            filteredProducts = allProducts.ToList();

            // Фильтр по поиску
            string search = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                filteredProducts = filteredProducts.Where(p =>
                    (p.Name != null && p.Name.ToLower().Contains(search)) ||
                    (p.ArticleNumber != null && p.ArticleNumber.ToLower().Contains(search)) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)))
                    .ToList();
            }

            // Фильтр по производителю
            if (cmbManufacturer.SelectedIndex > 0)
            {
                string manufacturer = cmbManufacturer.SelectedItem.ToString();
                filteredProducts = filteredProducts
                    .Where(p => p.Manufacturer == manufacturer)
                    .ToList();
            }

            // Фильтр по категории
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
                case "Наличие ↓":
                    filteredProducts = filteredProducts.OrderByDescending(p => p.StockQuantity).ToList();
                    break;
                default:
                    filteredProducts = filteredProducts.OrderBy(p => p.Name).ToList();
                    break;
            }

            // Обновляем результаты
            lblResults.Text = $"Найдено: {filteredProducts.Count} из {allProducts.Count}";

            // Отображаем товары
            DisplayProducts();
        }

        private void DisplayProducts()
        {
            flowPanel.SuspendLayout();
            flowPanel.Controls.Clear();

            if (filteredProducts.Count == 0)
            {
                // Сообщение об отсутствии товаров
                Panel noResults = new Panel
                {
                    Size = new Size(flowPanel.ClientSize.Width - 40, 150),
                    BackColor = LogoHelper.SecondaryColor,
                    BorderStyle = BorderStyle.FixedSingle
                };

                Label lblMessage = new Label
                {
                    Text = "Товары не найдены\nПопробуйте изменить критерии поиска",
                    Location = new Point(20, 50),
                    Size = new Size(400, 60),
                    Font = new Font("Comic Sans MS", 12),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                noResults.Controls.Add(lblMessage);
                flowPanel.Controls.Add(noResults);
            }
            else
            {
                // Рассчитываем ширину карточки
                int cardWidth = (flowPanel.ClientSize.Width - 80) / 3;
                if (cardWidth < 320) cardWidth = 320;
                if (cardWidth > 380) cardWidth = 380;

                foreach (var product in filteredProducts)
                {
                    Panel productCard = CreateProductCard(product, cardWidth);
                    flowPanel.Controls.Add(productCard);
                }
            }

            flowPanel.ResumeLayout(true);
        }

        private Panel CreateProductCard(Product product, int width)
        {
            int cardHeight = 460;
            Panel card = new Panel
            {
                Size = new Size(width - 20, cardHeight),
                Margin = new Padding(15, 10, 15, 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = product.StockQuantity > 0 ? Color.White : Color.FromArgb(245, 245, 245),
                Cursor = Cursors.Hand
            };

            // Изображение товара
            int imageHeight = 180;
            PictureBox pbImage = new PictureBox
            {
                Size = new Size(width - 40, imageHeight),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            LoadProductImage(pbImage, product);

            // Название товара
            Label lblName = new Label
            {
                Text = product.Name,
                Location = new Point(10, imageHeight + 15),
                Size = new Size(width - 40, 40),
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.TopLeft
            };

            // Производитель и категория
            Label lblInfo = new Label
            {
                Text = $"{product.Manufacturer} | {product.Category}",
                Location = new Point(10, imageHeight + 60),
                Size = new Size(width - 40, 20),
                Font = new Font("Comic Sans MS", 8),
                ForeColor = Color.Gray
            };

            // Цена
            decimal finalPrice = product.Price * (1 - product.CurrentDiscount / 100);
            Label lblPrice = new Label
            {
                Text = product.CurrentDiscount > 0
                    ? $"{finalPrice:C} (скидка {product.CurrentDiscount}%)"
                    : $"{product.Price:C}",
                Location = new Point(10, imageHeight + 85),
                Size = new Size(width - 40, 25),
                Font = new Font("Comic Sans MS", 11, FontStyle.Bold),
                ForeColor = product.CurrentDiscount > 0 ? Color.Red : LogoHelper.AccentColor
            };

            // Наличие
            Panel stockPanel = new Panel
            {
                Location = new Point(10, imageHeight + 115),
                Size = new Size(width - 40, 25),
                BackColor = product.StockQuantity > 0 ? Color.FromArgb(232, 255, 234) : Color.FromArgb(255, 232, 232)
            };

            Label lblStock = new Label
            {
                Text = product.StockQuantity > 0
                    ? $"✅ В наличии: {product.StockQuantity} {product.Unit}"
                    : "❌ Нет в наличии",
                Location = new Point(5, 3),
                Size = new Size(width - 50, 20),
                Font = new Font("Comic Sans MS", 8, FontStyle.Bold),
                ForeColor = product.StockQuantity > 0 ? Color.Green : Color.Red
            };

            stockPanel.Controls.Add(lblStock);

            // Кнопка добавления в корзину
            Button btnAddToCart = LogoHelper.CreateStyledButton(
                "В корзину",
                product.StockQuantity > 0 ? LogoHelper.AccentColor : Color.LightGray,
                product.StockQuantity > 0 ? Color.White : Color.DarkGray,
                width - 40, 35, new Font("Comic Sans MS", 9, FontStyle.Bold));
            btnAddToCart.Location = new Point(10, imageHeight + 145);
            btnAddToCart.Enabled = product.StockQuantity > 0;
            btnAddToCart.Tag = product;

            if (product.StockQuantity > 0)
            {
                btnAddToCart.Click += (s, e) =>
                {
                    Cart.AddProduct(product);
                    UpdateCartCount();
                    MessageBox.Show($"Товар '{product.Name}' добавлен в корзину!\nВсего в корзине: {Cart.TotalItems} товаров",
                        "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };
            }

            // Добавляем элементы на карточку
            card.Controls.AddRange(new Control[]
            {
                pbImage, lblName, lblInfo, lblPrice, stockPanel, btnAddToCart
            });

            return card;
        }

        private void LoadProductImage(PictureBox pictureBox, Product product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.ImagePath))
                {
                    SetDefaultImage(pictureBox);
                    return;
                }

                // Пробуем найти изображение по разным путям
                string[] searchPaths =
                {
            // 1. Прямой путь из БД
            product.ImagePath,
            
            // 2. В папке Resources/Images
            Path.Combine("Resources/Images/", product.ImagePath),
            
            // 3. В папке Images
            Path.Combine("Images/", product.ImagePath),
            
            // 4. В корне проекта
            product.ImagePath,
            
            // 5. Абсолютные пути
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/Images/", product.ImagePath),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, product.ImagePath),
            
            // 6. По артикулу в разных форматах
            Path.Combine("Resources/Images/", $"{product.ArticleNumber}.jpg"),
            Path.Combine("Resources/Images/", $"{product.ArticleNumber}.png"),
            Path.Combine("images/", $"{product.ArticleNumber}.jpg"),
            Path.Combine("images/", $"{product.ArticleNumber}.png")
        };

                foreach (var path in searchPaths)
                {
                    if (File.Exists(path))
                    {
                        pictureBox.Image = Image.FromFile(path);
                        return;
                    }
                }

                // Если изображение не найдено - используем заглушку
                SetDefaultImage(pictureBox);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                SetDefaultImage(pictureBox);
            }
        }

        private void SetDefaultImage(PictureBox pictureBox)
        {
            try
            {
                // Используем глобальную заглушку из LogoHelper
                pictureBox.Image = LogoHelper.GetPlaceholderImage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки заглушки: {ex.Message}");
                // Создаем простую заглушку программно
                Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.LightGray);
                    g.DrawString("Нет фото",
                        new Font("Comic Sans MS", 10),
                        Brushes.Gray, 10, 10);
                }
                pictureBox.Image = bmp;
            }
        }
      
        private void UpdateCartCount()
        {
            lblCartCount.Text = $"🛒 Корзина: {Cart.TotalItems} товаров";
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Логотип всегда в левом верхнем углу
            if (logoPictureBox != null)
            {
                logoPictureBox.Location = new Point(20, 10);
            }

            // Обновляем размер панели поиска
            Control searchPanel = this.Controls.OfType<Panel>().FirstOrDefault();
            if (searchPanel != null)
            {
                searchPanel.Width = this.ClientSize.Width;
            }

            // Обновляем положение элементов в панели поиска
            if (btnReset != null && lblCartCount != null && btnViewCart != null)
            {
                btnReset.Left = this.ClientSize.Width - btnReset.Width - 20;
                lblCartCount.Left = this.ClientSize.Width - lblCartCount.Width - 200;
                btnViewCart.Left = this.ClientSize.Width - btnViewCart.Width - 20;
            }

            // Обновляем размеры панели товаров
            if (flowPanel != null)
            {
                flowPanel.Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 200);

                if (filteredProducts != null && filteredProducts.Count > 0)
                {
                    DisplayProducts();
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ProductsForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "ProductsForm";
            this.Load += new System.EventHandler(this.ProductsForm_Load);
            this.ResumeLayout(false);

        }

        private void ProductsForm_Load(object sender, EventArgs e)
        {

        }
    }
}