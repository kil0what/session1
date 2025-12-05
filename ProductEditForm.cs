using System;
using System.Drawing;
using System.IO;
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
        private Button btnRemoveImage;
        private Label lblImageInfo;

        private string originalImagePath;
        private string newImagePath;
        private bool imageChanged = false;
        private bool isEditing;

        public ProductEditForm(Product existingProduct)
        {
            Product = existingProduct ?? new Product();
            isEditing = existingProduct != null;
            InitializeComponents();
            LoadProductData();
            LogoHelper.ApplyIcon(this)
            ;
        }

        private void InitializeComponents()
        {
            this.Text = isEditing ? "Редактирование товара" : "Добавление товара";
            this.Size = new Size(650, 750);
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
            int labelWidth = 150;
            int fieldWidth = 350;
            int fieldHeight = 25;

            // Артикул
            CreateLabel("Артикул:", yPos, labelWidth);
            txtArticle = CreateTextBox(yPos, fieldWidth, fieldHeight);
            txtArticle.MaxLength = 20;
            if (isEditing) txtArticle.Enabled = false;

            yPos += 35;

            // Название
            CreateLabel("Название товара:", yPos, labelWidth);
            txtName = CreateTextBox(yPos, fieldWidth, fieldHeight);
            txtName.MaxLength = 100;

            yPos += 35;

            // Категория
            CreateLabel("Категория:", yPos, labelWidth);
            cmbCategory = new ComboBox
            {
                Location = new Point(180, yPos),
                Size = new Size(fieldWidth, fieldHeight),
                DropDownStyle = ComboBoxStyle.DropDown
            };

            // Заполняем категории из БД
            LoadCategories();

            yPos += 35;

            // Количество
            CreateLabel("Количество на складе:", yPos, labelWidth);
            txtQuantity = CreateTextBox(yPos, fieldWidth, fieldHeight);
            txtQuantity.KeyPress += NumericTextBox_KeyPress;

            yPos += 35;

            // Единица измерения
            CreateLabel("Единица измерения:", yPos, labelWidth);
            txtUnit = CreateTextBox(yPos, fieldWidth, fieldHeight);
            txtUnit.Text = "шт.";

            yPos += 35;

            // Производитель
            CreateLabel("Производитель:", yPos, labelWidth);
            txtManufacturer = CreateTextBox(yPos, fieldWidth, fieldHeight);
            txtManufacturer.MaxLength = 50;

            yPos += 35;

            // Поставщик
            CreateLabel("Поставщик:", yPos, labelWidth);
            txtSupplier = CreateTextBox(yPos, fieldWidth, fieldHeight);
            txtSupplier.MaxLength = 50;

            yPos += 35;

            // Цена
            CreateLabel("Цена (руб.):", yPos, labelWidth);
            txtPrice = CreateTextBox(yPos, fieldWidth, fieldHeight);
            txtPrice.KeyPress += DecimalTextBox_KeyPress;

            yPos += 35;

            // Максимальная скидка
            CreateLabel("Макс. скидка (%):", yPos, labelWidth);
            txtMaxDiscount = CreateTextBox(yPos, fieldWidth, fieldHeight);
            txtMaxDiscount.KeyPress += NumericTextBox_KeyPress;
            txtMaxDiscount.Text = "0";

            yPos += 35;

            // Текущая скидка
            CreateLabel("Текущая скидка (%):", yPos, labelWidth);
            txtCurrentDiscount = CreateTextBox(yPos, fieldWidth, fieldHeight);
            txtCurrentDiscount.KeyPress += NumericTextBox_KeyPress;
            txtCurrentDiscount.Text = "0";

            yPos += 35;

            // Описание
            CreateLabel("Описание:", yPos, labelWidth);
            txtDescription = new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(fieldWidth, 80),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                MaxLength = 500
            };

            yPos += 90;

            // Изображение
            CreateLabel("Изображение:", yPos, labelWidth);

            // Информация об изображении
            lblImageInfo = new Label
            {
                Text = "Размер: 300x200 px",
                Location = new Point(180, yPos),
                Size = new Size(fieldWidth, 20),
                Font = new Font("Comic Sans MS", 8),
                ForeColor = Color.Gray
            };

            yPos += 25;

            picImage = new PictureBox
            {
                Location = new Point(180, yPos),
                Size = new Size(300, 200),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            btnBrowseImage = new Button
            {
                Text = "Выбрать изображение...",
                Location = new Point(490, yPos),
                Size = new Size(150, 30),
                BackColor = Color.FromArgb(73, 140, 81),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBrowseImage.Click += BtnBrowseImage_Click;

            btnRemoveImage = new Button
            {
                Text = "Удалить",
                Location = new Point(490, yPos + 40),
                Size = new Size(150, 30),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnRemoveImage.Click += BtnRemoveImage_Click;

            yPos += 220;

            // Кнопки
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(180, yPos),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(73, 140, 81),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Comic Sans MS", 10, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(310, yPos),
                Size = new Size(120, 40),
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.Cancel
            };

            // Добавляем все контролы
            mainPanel.Controls.AddRange(new Control[]
            {
                txtArticle, txtName, cmbCategory, txtQuantity, txtUnit,
                txtManufacturer, txtSupplier, txtPrice, txtMaxDiscount,
                txtCurrentDiscount, txtDescription, picImage, lblImageInfo,
                btnBrowseImage, btnRemoveImage, btnSave, btnCancel
            });

            this.Controls.Add(mainPanel);
            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void CreateLabel(string text, int yPos, int width)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(20, yPos),
                Size = new Size(width, 25),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Comic Sans MS", 10)
            };
            this.Controls.Add(label);
        }

        private TextBox CreateTextBox(int yPos, int width, int height)
        {
            return new TextBox
            {
                Location = new Point(180, yPos),
                Size = new Size(width, height),
                Font = new Font("Comic Sans MS", 10)
            };
        }

        private void LoadCategories()
        {
            try
            {
                // Получаем категории из БД
                var categories = DatabaseHelper.GetCategories();

                cmbCategory.Items.Clear();
                foreach (var category in categories)
                {
                    cmbCategory.Items.Add(category);
                }

                if (isEditing && !string.IsNullOrEmpty(Product.Category))
                {
                    if (cmbCategory.Items.Contains(Product.Category))
                    {
                        cmbCategory.SelectedItem = Product.Category;
                    }
                    else
                    {
                        cmbCategory.Items.Add(Product.Category);
                        cmbCategory.SelectedItem = Product.Category;
                    }
                }
                else if (cmbCategory.Items.Count > 0)
                {
                    cmbCategory.SelectedIndex = 0;
                }
            }
            catch
            {
                // Если не удалось загрузить категории, добавляем стандартные
                cmbCategory.Items.AddRange(new string[] {
                    "Спортивный инвентарь", "Одежда", "Обувь", "Аксессуары"
                });
                cmbCategory.SelectedIndex = 0;
            }
        }

        private void LoadProductData()
        {
            if (isEditing)
            {
                txtArticle.Text = Product.ArticleNumber;
                txtName.Text = Product.Name;
                txtQuantity.Text = Product.StockQuantity.ToString();
                txtUnit.Text = Product.Unit ?? "шт.";
                txtManufacturer.Text = Product.Manufacturer ?? "";
                txtSupplier.Text = Product.Supplier ?? "";
                txtPrice.Text = Product.Price.ToString("0.##");
                txtMaxDiscount.Text = Product.MaxDiscount.ToString("0.##");
                txtCurrentDiscount.Text = Product.CurrentDiscount.ToString("0.##");
                txtDescription.Text = Product.Description ?? "";

                // Сохраняем оригинальный путь к изображению
                originalImagePath = Product.ImagePath;

                // Загружаем изображение
                if (!string.IsNullOrEmpty(originalImagePath))
                {
                    LoadImageFromPath(originalImagePath);
                    btnRemoveImage.Enabled = true;
                }
                else
                {
                    SetDefaultImage();
                }
            }
            else
            {
                // Для нового товара
                SetDefaultImage();
            }
        }

        private void LoadImageFromPath(string imagePath)
        {
            try
            {
                string fullPath = GetFullImagePath(imagePath);
                if (File.Exists(fullPath))
                {
                    picImage.Image = Image.FromFile(fullPath);
                    lblImageInfo.Text = $"Изображение: {Path.GetFileName(imagePath)}";
                }
                else
                {
                    SetDefaultImage();
                }
            }
            catch
            {
                SetDefaultImage();
            }
        }

        private void SetDefaultImage()
        {
            try
            {
                // Используем глобальную заглушку
                picImage.Image = LogoHelper.GetPlaceholderImage();
                lblImageInfo.Text = "Используется изображение-заглушка";
            }
            catch
            {
                // Создаем простую заглушку программно
                Bitmap bmp = new Bitmap(296, 196);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.LightGray);
                    g.DrawString("Нет изображения",
                        new Font("Comic Sans MS", 14),
                        Brushes.Gray, 60, 80);
                }
                picImage.Image = bmp;
            }
        }

        private Image CreateDefaultImage()
        {
            Bitmap bmp = new Bitmap(300, 200);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                g.DrawString("Нет изображения",
                    new Font("Comic Sans MS", 12),
                    Brushes.Gray, 80, 80);
                g.DrawRectangle(Pens.DarkGray, 0, 0, 299, 199);
            }
            return bmp;
        }

        private string GetFullImagePath(string relativePath)
        {
            string[] possiblePaths =
            {
                Path.Combine("Resources/Images/", relativePath),
                Path.Combine("Images/", relativePath),
                Path.Combine("Resources/", relativePath),
                relativePath,
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/Images/", relativePath),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath)
            };

            foreach (var path in possiblePaths)
            {
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    return path;
                }
            }
            return relativePath;
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
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Выберите изображение товара";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Загружаем изображение
                        Image selectedImage = Image.FromFile(openFileDialog.FileName);

                        // Проверяем размер (300x200 по ТЗ)
                        if (selectedImage.Width > 300 || selectedImage.Height > 200)
                        {
                            DialogResult resizeResult = MessageBox.Show(
                                $"Изображение {selectedImage.Width}x{selectedImage.Height} px превышает рекомендуемый размер 300x200 px.\n" +
                                "Желаете уменьшить изображение?",
                                "Размер изображения",
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question);

                            if (resizeResult == DialogResult.Yes)
                            {
                                // Изменяем размер
                                selectedImage = ResizeImage(selectedImage, 300, 200);
                            }
                            else if (resizeResult == DialogResult.Cancel)
                            {
                                return;
                            }
                        }

                        picImage.Image = selectedImage;
                        newImagePath = Path.GetFileName(openFileDialog.FileName);
                        lblImageInfo.Text = $"Изображение: {newImagePath}";
                        imageChanged = true;
                        btnRemoveImage.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, width, height);
            }
            return result;
        }

        private void BtnRemoveImage_Click(object sender, EventArgs e)
        {
            SetDefaultImage();
            newImagePath = null;
            imageChanged = true;
            btnRemoveImage.Enabled = false;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            // Сохранение данных
            Product.ArticleNumber = txtArticle.Text.Trim();
            Product.Name = txtName.Text.Trim();
            Product.Category = cmbCategory.Text.Trim();
            Product.StockQuantity = int.Parse(txtQuantity.Text);
            Product.Unit = txtUnit.Text.Trim();
            Product.Manufacturer = txtManufacturer.Text.Trim();
            Product.Supplier = txtSupplier.Text.Trim();
            Product.Price = decimal.Parse(txtPrice.Text);
            Product.MaxDiscount = decimal.Parse(txtMaxDiscount.Text);
            Product.CurrentDiscount = decimal.Parse(txtCurrentDiscount.Text);
            Product.Description = txtDescription.Text.Trim();

            // Обработка изображения
            if (imageChanged)
            {
                // Удаляем старое изображение, если оно было
                if (!string.IsNullOrEmpty(originalImagePath))
                {
                    DeleteOldImage(originalImagePath);
                }

                // Сохраняем новое изображение
                if (!string.IsNullOrEmpty(newImagePath) && picImage.Image != null)
                {
                    string savedPath = SaveImageToFile();
                    Product.ImagePath = savedPath;
                }
                else
                {
                    Product.ImagePath = null; // Изображение было удалено
                }
            }
            else if (isEditing)
            {
                // Изображение не менялось, сохраняем старый путь
                Product.ImagePath = originalImagePath;
            }

            DialogResult = DialogResult.OK;
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

        private void DeleteOldImage(string oldImagePath)
        {
            try
            {
                if (!string.IsNullOrEmpty(oldImagePath))
                {
                    string[] possiblePaths =
                    {
                        Path.Combine("Resources/Images/", oldImagePath),
                        Path.Combine("Images/", oldImagePath),
                        oldImagePath
                    };

                    foreach (var path in possiblePaths)
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                            break;
                        }
                    }
                }
            }
            catch
            {
                // Игнорируем ошибки удаления
            }
        }

        private string SaveImageToFile()
        {
            try
            {
                // Создаем папку для изображений, если её нет
                string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                // Генерируем имя файла
                string fileName;
                if (!string.IsNullOrEmpty(newImagePath))
                {
                    fileName = $"{Product.ArticleNumber}_{Path.GetFileName(newImagePath)}";
                }
                else
                {
                    fileName = $"{Product.ArticleNumber}.jpg";
                }

                string fullPath = Path.Combine(imagesFolder, fileName);

                // Сохраняем изображение
                picImage.Image.Save(fullPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                return fileName; // Возвращаем только имя файла для хранения в БД
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения изображения: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }
        private void ProductEditForm_Load(object sender, EventArgs e)
        {

        }
    }
}