using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public partial class ManageProductsForm : Form
    {
        private DataGridView dataGridView;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnBack;
        private TextBox txtSearch;
        private ComboBox cmbCategoryFilter;
        private Label lblTitle;
        private PictureBox logoPictureBox;

        public ManageProductsForm()
        {
            // Проверка прав администратора
            if (Session.CurrentUser == null || Session.CurrentUser.Role != "Администратор")
            {
                MessageBox.Show("Доступ запрещен. Требуются права администратора.",
                    "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            InitializeComponents();
            LoadProductsData();
            ApplyStyles();
            LogoHelper.ApplyIcon(this)
            ;
        }

        private void InitializeComponents()
        {
            this.Text = "ООО Спортивные товары - Управление товарами";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Font = new Font("Comic Sans MS", 10);
            this.MinimumSize = new Size(1000, 600);

            // ЛОГОТИП (левый верхний угол)
            logoPictureBox = LogoHelper.CreateLogo();

            // Заголовок
            lblTitle = new Label
            {
                Text = "⚙️ Управление товарами",
                Location = new Point(180, 15),
                Size = new Size(300, 40),
                Font = new Font("Comic Sans MS", 16, FontStyle.Bold),
                ForeColor = LogoHelper.AccentColor
            };

            // Панель инструментов
            Panel toolPanel = new Panel
            {
                Location = new Point(0, 70),
                Size = new Size(this.ClientSize.Width, 80),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Поиск
            var lblSearch = new Label
            {
                Text = "Поиск:",
                Location = new Point(20, 25),
                Size = new Size(60, 25),
                Font = LogoHelper.MainFont
            };

            txtSearch = new TextBox
            {
                Location = new Point(85, 25),
                Size = new Size(200, 25),
                Font = LogoHelper.MainFont
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Фильтр по категории
            var lblCategory = new Label
            {
                Text = "Категория:",
                Location = new Point(300, 25),
                Size = new Size(80, 25),
                Font = LogoHelper.MainFont
            };

            cmbCategoryFilter = new ComboBox
            {
                Location = new Point(385, 25),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = LogoHelper.MainFont
            };
            cmbCategoryFilter.SelectedIndexChanged += CmbCategoryFilter_SelectedIndexChanged;

            // Кнопки управления
            btnAdd = new Button
            {
                Text = "➕ Добавить товар",
                Location = new Point(550, 20),
                Size = new Size(150, 35),
                BackColor = LogoHelper.AccentColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold)
            };
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button
            {
                Text = "✏️ Редактировать",
                Location = new Point(710, 20),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(66, 135, 245),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold)
            };
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "🗑️ Удалить",
                Location = new Point(870, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(245, 66, 66),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold)
            };
            btnDelete.Click += BtnDelete_Click;

            btnRefresh = new Button
            {
                Text = "🔄 Обновить",
                Location = new Point(1000, 20),
                Size = new Size(120, 35),
                BackColor = LogoHelper.SecondaryColor,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Comic Sans MS", 10)
            };
            btnRefresh.Click += (s, e) => LoadProductsData();

            toolPanel.Controls.AddRange(new Control[]
            {
                lblSearch, txtSearch, lblCategory, cmbCategoryFilter,
                btnAdd, btnEdit, btnDelete, btnRefresh
            });

            // DataGridView
            dataGridView = new DataGridView
            {
                Location = new Point(20, 160),
                Size = new Size(this.ClientSize.Width - 40, this.ClientSize.Height - 230),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                Font = new Font("Comic Sans MS", 9)
            };

            // Настройка столбцов
            SetupDataGridViewColumns();

            // Панель с кнопками
            Panel bottomPanel = new Panel
            {
                Location = new Point(0, this.ClientSize.Height - 60),
                Size = new Size(this.ClientSize.Width, 60),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            btnBack = new Button
            {
                Text = "← Назад",
                Location = new Point(20, 15),
                Size = new Size(120, 30),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Comic Sans MS", 10)
            };
            btnBack.Click += (s, e) => this.Close();

            var lblInfo = new Label
            {
                Text = "Администратор: " + Session.CurrentUser.FullName,
                Location = new Point(200, 20),
                Size = new Size(300, 20),
                Font = new Font("Comic Sans MS", 9),
                ForeColor = Color.Gray
            };

            var lblCount = new Label
            {
                Text = "Товаров: 0",
                Location = new Point(500, 20),
                Size = new Size(200, 20),
                Font = new Font("Comic Sans MS", 9, FontStyle.Bold),
                ForeColor = LogoHelper.AccentColor
            };

            bottomPanel.Controls.AddRange(new Control[] { btnBack, lblInfo, lblCount });

            this.Controls.AddRange(new Control[] {
                logoPictureBox, lblTitle, toolPanel, dataGridView, bottomPanel
            });
        }

        private void SetupDataGridViewColumns()
        {
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add("ArticleNumber", "Артикул");
            dataGridView.Columns.Add("Name", "Название");
            dataGridView.Columns.Add("Category", "Категория");
            dataGridView.Columns.Add("Manufacturer", "Производитель");
            dataGridView.Columns.Add("Price", "Цена");
            dataGridView.Columns.Add("StockQuantity", "На складе");
            dataGridView.Columns.Add("Unit", "Ед. изм.");
            dataGridView.Columns.Add("Supplier", "Поставщик");
            dataGridView.Columns.Add("CurrentDiscount", "Скидка %");
            dataGridView.Columns.Add("MaxDiscount", "Макс. скидка %");

            dataGridView.Columns["Price"].DefaultCellStyle.Format = "C2";
            dataGridView.Columns["CurrentDiscount"].DefaultCellStyle.Format = "0.##'%'";
            dataGridView.Columns["MaxDiscount"].DefaultCellStyle.Format = "0.##'%'";

            // Настройка ширины столбцов
            dataGridView.Columns["ArticleNumber"].Width = 100;
            dataGridView.Columns["Name"].Width = 200;
            dataGridView.Columns["Category"].Width = 150;
            dataGridView.Columns["Manufacturer"].Width = 150;
            dataGridView.Columns["Price"].Width = 100;
            dataGridView.Columns["StockQuantity"].Width = 80;
            dataGridView.Columns["Unit"].Width = 70;
            dataGridView.Columns["Supplier"].Width = 150;
            dataGridView.Columns["CurrentDiscount"].Width = 80;
            dataGridView.Columns["MaxDiscount"].Width = 80;
        }

        private void ApplyStyles()
        {
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Comic Sans MS", 10, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = LogoHelper.AccentColor;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            // Подсветка товаров с низким остатком
            dataGridView.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == dataGridView.Columns["StockQuantity"].Index && e.RowIndex >= 0)
                {
                    if (dataGridView.Rows[e.RowIndex].Cells["StockQuantity"].Value != null)
                    {
                        int stock = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["StockQuantity"].Value);
                        if (stock <= 3)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 200, 200);
                            e.CellStyle.ForeColor = Color.Red;
                        }
                        else if (stock == 0)
                        {
                            e.CellStyle.BackColor = Color.FromArgb(255, 150, 150);
                            e.CellStyle.ForeColor = Color.Red;
                        }
                    }
                }
            };
        }

        private void LoadProductsData()
        {
            try
            {
                // Получаем товары из БД
                var products = DatabaseHelper.GetAllProducts();

                dataGridView.Rows.Clear();

                foreach (var product in products)
                {
                    dataGridView.Rows.Add(
                        product.ArticleNumber,
                        product.Name,
                        product.Category,
                        product.Manufacturer,
                        product.Price,
                        product.StockQuantity,
                        product.Unit,
                        product.Supplier,
                        product.CurrentDiscount,
                        product.MaxDiscount
                    );
                }

                // Заполняем фильтр категорий
                var categories = products
                    .Select(p => p.Category)
                    .Where(c => !string.IsNullOrEmpty(c))
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();

                cmbCategoryFilter.Items.Clear();
                cmbCategoryFilter.Items.Add("Все категории");
                foreach (var category in categories)
                {
                    cmbCategoryFilter.Items.Add(category);
                }
                cmbCategoryFilter.SelectedIndex = 0;

                // Обновляем счетчик
                UpdateProductCount(products.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateProductCount(int count)
        {
            // Находим метку с количеством товаров
            foreach (Control control in this.Controls)
            {
                if (control is Panel panel)
                {
                    foreach (Control panelControl in panel.Controls)
                    {
                        if (panelControl is Label label && label.Text.StartsWith("Товаров:"))
                        {
                            label.Text = $"Товаров: {count}";
                            return;
                        }
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var editForm = new ProductEditForm(null))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Сохраняем товар в БД
                        bool success = DatabaseHelper.AddProduct(editForm.Product);

                        if (success)
                        {
                            MessageBox.Show("Товар успешно добавлен", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProductsData(); // Обновляем список
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления товара: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для редактирования",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var selectedRow = dataGridView.SelectedRows[0];
                var articleNumber = selectedRow.Cells["ArticleNumber"].Value.ToString();

                // Получаем товар из БД
                var product = DatabaseHelper.GetProductByArticle(articleNumber);

                if (product != null)
                {
                    using (var editForm = new ProductEditForm(product))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            // Обновляем товар в БД
                            bool success = DatabaseHelper.UpdateProduct(editForm.Product);

                            if (success)
                            {
                                MessageBox.Show("Товар успешно обновлен", "Успех",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadProductsData(); // Обновляем список
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Товар не найден в базе данных",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка редактирования товара: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для удаления",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var selectedRow = dataGridView.SelectedRows[0];
                var articleNumber = selectedRow.Cells["ArticleNumber"].Value.ToString();
                var productName = selectedRow.Cells["Name"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Вы действительно хотите удалить товар?\n\n" +
                    $"Название: {productName}\n" +
                    $"Артикул: {articleNumber}\n\n" +
                    "Внимание: Это действие нельзя отменить!",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Удаляем товар из БД
                    bool success = DatabaseHelper.DeleteProduct(articleNumber);

                    if (success)
                    {
                        MessageBox.Show("Товар успешно удален", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProductsData(); // Обновляем список
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления товара: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void CmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterProducts();
        }

        private void FilterProducts()
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            string selectedCategory = cmbCategoryFilter.SelectedItem?.ToString();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                bool visible = true;

                if (!string.IsNullOrEmpty(searchText))
                {
                    string name = row.Cells["Name"].Value?.ToString()?.ToLower() ?? "";
                    string article = row.Cells["ArticleNumber"].Value?.ToString()?.ToLower() ?? "";
                    string manufacturer = row.Cells["Manufacturer"].Value?.ToString()?.ToLower() ?? "";
                    string supplier = row.Cells["Supplier"].Value?.ToString()?.ToLower() ?? "";

                    visible = name.Contains(searchText) ||
                              article.Contains(searchText) ||
                              manufacturer.Contains(searchText) ||
                              supplier.Contains(searchText);
                }

                if (visible && selectedCategory != "Все категории" && !string.IsNullOrEmpty(selectedCategory))
                {
                    string category = row.Cells["Category"].Value?.ToString() ?? "";
                    visible = category == selectedCategory;
                }

                row.Visible = visible;
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

            // Обновляем размеры DataGridView
            if (dataGridView != null)
            {
                dataGridView.Width = this.ClientSize.Width - 40;
                dataGridView.Height = this.ClientSize.Height - 230;
            }

            // Обновляем ширину панели инструментов
            Control toolPanel = this.Controls.OfType<Panel>().FirstOrDefault();
            if (toolPanel != null)
            {
                toolPanel.Width = this.ClientSize.Width;
            }

            // Обновляем нижнюю панель
            Control bottomPanel = this.Controls.OfType<Panel>().LastOrDefault();
            if (bottomPanel != null)
            {
                bottomPanel.Location = new Point(0, this.ClientSize.Height - 60);
                bottomPanel.Width = this.ClientSize.Width;
            }
        }
    }
}