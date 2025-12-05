using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public static class LogoHelper
    {
        // Пути к файлам
        private static string logoPath = @"C:\Users\Lenovo\Downloads\Задание практика ПМ05\Практика ПМ 03-ПМ05\Задание на практику\Вариант 2\Общие ресурсы\logo.png";
        private static string iconPath = @"C:\Users\Lenovo\source\repos\session1\images\icon.ico";
        private static string placeholderPath = @"C:\Users\Lenovo\source\repos\session1\images\picture.png";

        // Стандартная позиция логотипа (левый верхний угол)
        private static Point logoPosition = new Point(20, 10);
        private static Size logoSize = new Size(150, 40);

        // Цвета из ТЗ
        public static Color AccentColor = Color.FromArgb(73, 140, 81);
        public static Color SecondaryColor = Color.FromArgb(118, 227, 131);
        public static Color BackgroundColor = Color.White;
        public static Font MainFont = new Font("Comic Sans MS", 10);

        // Иконка приложения
        private static Icon appIcon = null;

        // Получает иконку приложения
        public static Icon GetAppIcon()
        {
            if (appIcon == null)
            {
                LoadAppIcon();
            }
            return appIcon;
        }

        // Загружает иконку приложения
        private static void LoadAppIcon()
        {
            try
            {
                if (File.Exists(iconPath))
                {
                    appIcon = new Icon(iconPath);
                    return;
                }

                // Альтернативные пути к иконке
                string[] possibleIconPaths =
                {
                    "images/icon.ico",
                    "Resources/icon.ico",
                    "icon.ico",
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images/icon.ico"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/icon.ico"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icon.ico")
                };

                foreach (var path in possibleIconPaths)
                {
                    if (File.Exists(path))
                    {
                        appIcon = new Icon(path);
                        return;
                    }
                }

                // Создаем простую иконку если файл не найден
                CreateDefaultIcon();
            }
            catch
            {
                CreateDefaultIcon();
            }
        }

        // Создает простую иконку если файл не найден
        private static void CreateDefaultIcon()
        {
            // Создаем простую иконку 32x32
            Bitmap bmp = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(SecondaryColor);
                g.FillEllipse(new SolidBrush(AccentColor), 4, 4, 24, 24);
                g.DrawString("ST", new Font("Comic Sans MS", 10, FontStyle.Bold),
                    Brushes.White, 6, 8);
            }

            // Конвертируем Bitmap в Icon
            IntPtr hIcon = bmp.GetHicon();
            appIcon = Icon.FromHandle(hIcon);
        }

        // Применяет иконку к форме
        public static void ApplyIcon(Form form)
        {
            try
            {
                form.Icon = GetAppIcon();
            }
            catch
            {
                // Игнорируем ошибки
            }
        }

        // Создает PictureBox с логотипом в стандартной позиции
        public static PictureBox CreateLogo()
        {
            PictureBox logoPictureBox = new PictureBox
            {
                Size = logoSize,
                Location = logoPosition,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };

            LoadLogoImage(logoPictureBox);

            return logoPictureBox;
        }

        // Загружает изображение логотипа
        public static void LoadLogoImage(PictureBox pictureBox)
        {
            try
            {
                if (File.Exists(logoPath))
                {
                    pictureBox.Image = Image.FromFile(logoPath);
                    return;
                }

                // Альтернативные пути
                string[] possiblePaths =
                {
                    "Resources/logo.png",
                    "logo.png",
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/logo.png"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png")
                };

                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        pictureBox.Image = Image.FromFile(path);
                        return;
                    }
                }

                // Создаем логотип по умолчанию
                CreateDefaultLogo(pictureBox);
            }
            catch
            {
                CreateDefaultLogo(pictureBox);
            }
        }

        // Создает простой логотип если файл не найден
        private static void CreateDefaultLogo(PictureBox pictureBox)
        {
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(SecondaryColor);

                // Рамка
                g.DrawRectangle(new Pen(AccentColor, 2), 0, 0, bmp.Width - 1, bmp.Height - 1);

                // Текст
                string logoText = "Спортивные товары";
                Font font = new Font("Comic Sans MS", 9, FontStyle.Bold);
                SizeF textSize = g.MeasureString(logoText, font);

                float x = (bmp.Width - textSize.Width) / 2;
                float y = (bmp.Height - textSize.Height) / 2;

                g.DrawString(logoText, font, Brushes.Black, x, y);
            }
            pictureBox.Image = bmp;
        }

        // Получает изображение-заглушку для товаров
        public static Image GetPlaceholderImage()
        {
            try
            {
                if (File.Exists(placeholderPath))
                {
                    return Image.FromFile(placeholderPath);
                }

                // Альтернативные пути
                string[] possiblePaths =
                {
                    "images/picture.png",
                    "Resources/picture.png",
                    "picture.png",
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images/picture.png"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/picture.png"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "picture.png")
                };

                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        return Image.FromFile(path);
                    }
                }

                // Создаем заглушку по умолчанию
                return CreateDefaultPlaceholder();
            }
            catch
            {
                return CreateDefaultPlaceholder();
            }
        }

        // Создает заглушку по умолчанию
        private static Image CreateDefaultPlaceholder()
        {
            Bitmap bmp = new Bitmap(300, 200);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);

                // Иконка фотоаппарата
                g.DrawString("📷", new Font("Segoe UI Emoji", 48),
                    Brushes.Gray, 100, 50);

                // Текст
                g.DrawString("Нет изображения",
                    new Font("Comic Sans MS", 14),
                    Brushes.DarkGray, 70, 140);

                // Рамка
                g.DrawRectangle(Pens.DarkGray, 0, 0, 299, 199);
            }
            return bmp;
        }

        // Применяет стандартные стили к форме
        public static void ApplyDefaultStyles(Form form)
        {
            form.BackColor = BackgroundColor;
            form.Font = MainFont;

            // Применяем иконку
            ApplyIcon(form);
        }

        // Создает стилизованную кнопку
        public static Button CreateStyledButton(string text, Color backColor, Color foreColor,
            int width = 120, int height = 40, Font font = null)
        {
            if (font == null)
                font = new Font("Comic Sans MS", 10, FontStyle.Bold);

            return new Button
            {
                Text = text,
                Size = new Size(width, height),
                Font = font,
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                TextAlign = ContentAlignment.MiddleCenter
            };
        }
    }
}