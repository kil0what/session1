using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public partial class ProductEditForm : Form
    {
        public Product Product { get; private set; }

        private TextBox txtArticle;
        private TextBox txtName;
        private ComboBox cmbCategory;
        private TextBox txtQuantity;
        private TextBox txtUnit;
        private TextBox txtManufacturer;
        private TextBox txtSupplier;
        private TextBox txtPrice;
        private TextBox txtMaxDiscount;
        private TextBox txtCurrentDiscount;
        private TextBox txtDescription;
        private PictureBox picImage;
        private Button btnBrowseImage;
        private Button btnSave;
        private Button btnCancel;

        private string selectedImagePath;
        private bool isEditing;

        public ProductEditForm(Product existingProduct)
        {
            Product = existingProduct ?? new Product();
            isEditing = existingProduct != null;
            InitializeComponents();
            LoadProductData();
            ApplyStyles();
        }

        private void InitializeComponents()
        {
            this.Text = isEditing ? "Редактирование товара" : "Добавление товара";
            this.Size = new Size(600, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Comic Sans MS", 10);

            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            int yPos = 20;
            int fieldWidth = 350;

            // Артикул
            CreateLabel("Артикул:", yPos);
            txtArticle = CreateTextBox(yPos, fieldWidth);
            txtArticle.MaxLength = 20;

            yPos += 40;

            // Название
            CreateLabel("Название товара:", yPos);
            txtName = CreateTextBox(yPos, fieldWidth);
            txtName.MaxLength = 100;

            yPos += 40;

            // Категория
            CreateLabel("Категория:", yPos);
            cmbCategory = new ComboBox
            {
                Location = new Point(180, yPos),
                Size = new Size(fieldWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // Заполняем категории
            var categories = FakeDatabase.Products
                .Select(p => p.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .OrderBy(c => c)
                .ToArray();

            cmbCategory.Items.AddRange(categories);
            cmbCategory.Items.Add("Другая категория");

            yPos += 40;

            // Количество
            CreateLabel("Количество на складе:", yPos);
            txtQuantity = CreateTextBox(yPos, fieldWidth);
            txtQuantity.KeyPress += NumericTextBox_KeyPress;

            yPos += 40;

            // Единица измерения
            CreateLabel("Единица измерения:", yPos);
            txtUnit = CreateTextBox(yPos, fieldWidth);
            txtUnit.Text = "шт.";

            yPos += 40;

            // Производитель
            CreateLabel("Производитель:", yPos);
            txtManufacturer = CreateTextBox(yPos, fieldWidth);
            txtManufacturer.MaxLength = 50;

            yPos += 40;

            // Поставщик
            CreateLabel("Поставщик:", yPos);
            txtSupplier = CreateTextBox(yPos, fieldWidth);
            txtSupplier.MaxLength = 50;

            yPos += 40;

            // Цена
            CreateLabel("Цена (руб.):", yPos);
            txtPrice = CreateTextBox(yPos, fieldWidth);
            txtPrice.KeyPress += DecimalTextBox_KeyPress;

            yPos += 40;

            // Максимальная скидка
            CreateLabel("Макс. скидка (%):", yPos);
            txtMaxDiscount = CreateTextBox(yPos, fieldWidth);
            txtMaxDiscount.KeyPress += NumericTextBox_KeyPress;
            txtMaxDiscount.Text = "0";

            yPos += 40;

            // Текущая скидка
            CreateLabel("Текущая скидка (%):", yPos);
            txtCurrentDiscount = CreateTextBox(yPos, fieldWidth);
            txtCurrentDiscount.KeyPress += NumericTextBox_KeyPress;
            txtCurrentDiscount.Text = "0";

            yPos += 40;

            // Описание
            CreateLabel("Описание:", yPos);
            txtDescription = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(fieldWidth, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                MaxLength = 500
            };

            yPos += 100;

            // Изображение
            CreateLabel("Изображение:", yPos);
            picImage = new PictureBox
            {
                Location = new Point(180, yPos),
                Size = new Size(150, 150),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            btnBrowseImage = new Button
            {
                Text = "Выбрать...",
                Location = new Point(340, yPos),
                Size = new Size(100, 30),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat
            };
            btnBrowseImage.Click += BtnBrowseImage_Click;

            yPos += 170;

            // Кнопки
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(180, yPos),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(73, 140, 81),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(310, yPos),
                Size = new Size(120, 35),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };

            // Добавляем все контролы
            mainPanel.Controls.AddRange(new Control[]
            {
                txtArticle,
                txtName,
                cmbCategory,
                txtQuantity,
                txtUnit,
                txtManufacturer,
                txtSupplier,
                txtPrice,
                txtMaxDiscount,
                txtCurrentDiscount,
                txtDescription,
                picImage, btnBrowseImage,
                btnSave, btnCancel
            });

            this.Controls.Add(mainPanel);
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void CreateLabel(string text, int yPos)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(20, yPos),
                Size = new Size(150, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            this.Controls.Add(label);
        }

        private TextBox CreateTextBox(int yPos, int width)
        {
            return new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(width, 25)
            };
        }

        private void LoadProductData()
        {
            if (isEditing)
            {
                txtArticle.Text = Product.ArticleNumber;
                txtArticle.Enabled = false;
                txtName.Text = Product.Name;

                if (cmbCategory.Items.Contains(Product.Category))
                    cmbCategory.SelectedItem = Product.Category;
                else if (cmbCategory.Items.Count > 0)
                    cmbCategory.SelectedIndex = cmbCategory.Items.Count - 1;

                txtQuantity.Text = Product.StockQuantity.ToString();
                txtUnit.Text = Product.Unit;
                txtManufacturer.Text = Product.Manufacturer;
                txtSupplier.Text = Product.Supplier;
                txtPrice.Text = Product.Price.ToString("0.##");
                txtMaxDiscount.Text = Product.MaxDiscount.ToString("0.##");
                txtCurrentDiscount.Text = Product.CurrentDiscount.ToString("0.##");
                txtDescription.Text = Product.Description;

                // Загружаем изображение
                if (!string.IsNullOrEmpty(Product.ImagePath))
                {
                    string imagePath = Path.Combine("Resources/Images/", Product.ImagePath);
                    if (File.Exists(imagePath))
                    {
                        try
                        {
                            picImage.Image = Image.FromFile(imagePath);
                            selectedImagePath = Product.ImagePath;
                        }
                        catch { }
                    }
                }
            }
            else
            {
                // Для нового товара генерируем артикул
                txtArticle.Text = GenerateArticleNumber();
            }
        }

        private string GenerateArticleNumber()
        {
            Random rand = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] result = new char[6];
            for (int i = 0; i < 6; i++)
            {
                result[i] = chars[rand.Next(chars.Length)];
            }
            return new string(result);
        }

        private void ApplyStyles()
        {
            btnSave.Font = new Font("Comic Sans MS", 10, FontStyle.Bold);
            btnCancel.Font = new Font("Comic Sans MS", 10);
        }

        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void DecimalTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Только одна запятая
            if (e.KeyChar == ',' && ((TextBox)sender).Text.Contains(","))
            {
                e.Handled = true;
            }
        }

        private void BtnBrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.Title = "Выберите изображение товара";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        picImage.Image = Image.FromFile(openFileDialog.FileName);
                        selectedImagePath = Path.GetFileName(openFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Валидация
            if (!ValidateInput())
                return;

            // Сохранение данных
            Product.ArticleNumber = txtArticle.Text.Trim();
            Product.Name = txtName.Text.Trim();
            Product.Category = cmbCategory.SelectedItem?.ToString() ?? "Другое";
            Product.StockQuantity = int.Parse(txtQuantity.Text);
            Product.Unit = txtUnit.Text.Trim();
            Product.Manufacturer = txtManufacturer.Text.Trim();
            Product.Supplier = txtSupplier.Text.Trim();
            Product.Price = decimal.Parse(txtPrice.Text);
            Product.MaxDiscount = decimal.Parse(txtMaxDiscount.Text);
            Product.CurrentDiscount = decimal.Parse(txtCurrentDiscount.Text);
            Product.Description = txtDescription.Text.Trim();

            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                Product.ImagePath = selectedImagePath;
            }

            DialogResult = DialogResult.OK;
        }
        private void ProductEditForm_Load(object sender, EventArgs e)
        {
            // Оставляем пустым
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtArticle.Text))
            {
                MessageBox.Show("Введите артикул товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtArticle.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Введите корректное количество (неотрицательное число)", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQuantity.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Введите корректную цену (неотрицательное число)", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPrice.Focus();
                return false;
            }

            return true;
        }
    }
}