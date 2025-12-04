using System;
using System.Drawing;
using System.IO;
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
        private Button btnSave;
        private Button btnBack;
        private TextBox txtSearch;
        private ComboBox cmbCategoryFilter;

        private static bool isEditFormOpen = false; // Блокировка открытия нескольких окон

        public ManageProductsForm()
        {
            if (!Session.IsAdmin)
            {
                MessageBox.Show("Доступ запрещен. Требуются права администратора.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            InitializeComponents();
            LoadProducts();
            ApplyStyles();
        }

        private void InitializeComponents()
        {
            this.Text = "Управление товарами";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Font = new Font("Comic Sans MS", 10);
            this.MinimumSize = new Size(800, 500);

            // Панель инструментов
            Panel toolPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Поиск
            var lblSearch = new Label
            {
                Text = "Поиск:",
                Location = new Point(20, 25),
                Size = new Size(60, 25)
            };

            txtSearch = new TextBox
            {
                Location = new Point(85, 25),
                Size = new Size(200, 25)
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Фильтр по категории
            var lblCategory = new Label
            {
                Text = "Категория:",
                Location = new Point(300, 25),
                Size = new Size(80, 25)
            };

            cmbCategoryFilter = new ComboBox
            {
                Location = new Point(385, 25),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategoryFilter.Items.Add("Все категории");
            cmbCategoryFilter.SelectedIndex = 0;
            cmbCategoryFilter.SelectedIndexChanged += CmbCategoryFilter_SelectedIndexChanged;

            // Кнопки управления
            btnAdd = new Button
            {
                Text = "➕ Добавить",
                Location = new Point(550, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(73, 140, 81),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button
            {
                Text = "✏️ Редактировать",
                Location = new Point(680, 20),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(66, 135, 245),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "🗑️ Удалить",
                Location = new Point(830, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(245, 66, 66),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.Click += BtnDelete_Click;

            toolPanel.Controls.AddRange(new Control[]
            {
                lblSearch, txtSearch, lblCategory, cmbCategoryFilter,
                btnAdd, btnEdit, btnDelete
            });

            // DataGridView
            dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                Location = new Point(0, 80),
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height - 130),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false
            };

            // Панель с кнопками
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            btnSave = new Button
            {
                Text = "💾 Сохранить изменения",
                Location = new Point(20, 10),
                Size = new Size(180, 30),
                BackColor = Color.FromArgb(118, 227, 131),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;

            btnBack = new Button
            {
                Text = "Назад",
                Location = new Point(880, 10),
                Size = new Size(100, 30),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnBack.Click += (s, e) => this.Close();

            bottomPanel.Controls.AddRange(new Control[] { btnSave, btnBack });

            this.Controls.AddRange(new Control[] { toolPanel, dataGridView, bottomPanel });
        }

        private void ApplyStyles()
        {
            dataGridView.Font = new Font("Comic Sans MS", 9);
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Comic Sans MS", 10, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(73, 140, 81);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
        }
        private void ManageProductsForm_Load(object sender, EventArgs e)
        {
            // Оставляем пустым
        }
        private void LoadProducts()
        {
            dataGridView.Columns.Clear();

            // Настройка столбцов
            dataGridView.Columns.Add("ArticleNumber", "Артикул");
            dataGridView.Columns.Add("Name", "Название");
            dataGridView.Columns.Add("Category", "Категория");
            dataGridView.Columns.Add("Manufacturer", "Производитель");
            dataGridView.Columns.Add("Price", "Цена");
            dataGridView.Columns.Add("StockQuantity", "На складе");
            dataGridView.Columns.Add("Unit", "Ед. изм.");
            dataGridView.Columns.Add("Supplier", "Поставщик");
            dataGridView.Columns.Add("CurrentDiscount", "Скидка %");

            dataGridView.Columns["Price"].DefaultCellStyle.Format = "C2";
            dataGridView.Columns["CurrentDiscount"].DefaultCellStyle.Format = "0.##'%'";

            // Заполнение данными
            foreach (var product in FakeDatabase.Products)
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
                    product.CurrentDiscount
                );
            }

            // Заполняем фильтр категорий
            var categories = FakeDatabase.Products
                .Select(p => p.Category)
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
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (isEditFormOpen)
            {
                MessageBox.Show("Уже открыто окно редактирования. Закройте его перед созданием нового.",
                    "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            isEditFormOpen = true;
            using (var editForm = new ProductEditForm(null))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    FakeDatabase.Products.Add(editForm.Product);
                    LoadProducts();
                }
            }
            isEditFormOpen = false;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для редактирования",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (isEditFormOpen)
            {
                MessageBox.Show("Уже открыто окно редактирования",
                    "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridView.SelectedRows[0];
            var articleNumber = selectedRow.Cells["ArticleNumber"].Value.ToString();
            var product = FakeDatabase.Products.FirstOrDefault(p => p.ArticleNumber == articleNumber);

            if (product != null)
            {
                isEditFormOpen = true;
                using (var editForm = new ProductEditForm(product))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadProducts();
                    }
                }
                isEditFormOpen = false;
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

            var selectedRow = dataGridView.SelectedRows[0];
            var articleNumber = selectedRow.Cells["ArticleNumber"].Value.ToString();
            var product = FakeDatabase.Products.FirstOrDefault(p => p.ArticleNumber == articleNumber);

            if (product != null)
            {
                // Проверяем, есть ли товар в заказах
                bool isInOrder = FakeDatabase.Orders.Any(o =>
                    o.OrderItems.Contains(product.ArticleNumber));

                if (isInOrder)
                {
                    MessageBox.Show($"Товар '{product.Name}' присутствует в заказе и не может быть удален.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var result = MessageBox.Show($"Вы действительно хотите удалить товар '{product.Name}'?\n\n" +
                    "Внимание: Все дополнительные товары также будут удалены.",
                    "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    FakeDatabase.Products.Remove(product);

                    // TODO: Удалить изображение товара из папки
                    if (!string.IsNullOrEmpty(product.ImagePath) && File.Exists($"Resources/Images/{product.ImagePath}"))
                    {
                        try { File.Delete($"Resources/Images/{product.ImagePath}"); }
                        catch { /* Игнорируем ошибки удаления файла */ }
                    }

                    LoadProducts();
                    MessageBox.Show("Товар успешно удален", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Все изменения сохранены", "Информация",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                    visible = name.Contains(searchText) ||
                              article.Contains(searchText) ||
                              manufacturer.Contains(searchText);
                }

                if (visible && selectedCategory != "Все категории")
                {
                    string category = row.Cells["Category"].Value?.ToString() ?? "";
                    visible = category == selectedCategory;
                }

                row.Visible = visible;
            }
        }
    }
}