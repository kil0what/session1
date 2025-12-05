using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Создаем необходимые папки для приложения
            CreateApplicationFolders();

            // Проверяем подключение к базе данных
            bool dbConnected = CheckDatabaseConnection();

            if (dbConnected)
            {
                // Показываем загрузочный экран
                ShowSplashScreen();

                // Импортируем пользователей если нужно
                ImportUsers();

                // Запускаем главную форму
                RunApplication();
            }
            else
            {
                ShowDatabaseError();
            }
        }



        /// <summary>
        /// Создает необходимые папки для работы приложения
        /// </summary>
        private static void CreateApplicationFolders()
        {
            try
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Папка Resources
                string resourcesPath = Path.Combine(baseDirectory, "Resources");
                if (!Directory.Exists(resourcesPath))
                {
                    Directory.CreateDirectory(resourcesPath);
                    Console.WriteLine($"Создана папка: {resourcesPath}");
                }

                // Папка для изображений товаров
                string imagesPath = Path.Combine(resourcesPath, "Images");
                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                    Console.WriteLine($"Создана папка: {imagesPath}");
                }

                // Папка для логов (опционально)
                string logsPath = Path.Combine(baseDirectory, "Logs");
                if (!Directory.Exists(logsPath))
                {
                    Directory.CreateDirectory(logsPath);
                }

                // Создаем файл-заглушку для изображений
                CreateDefaultImage(imagesPath);

                // Копируем логотип если есть
                CopyLogoIfExists(resourcesPath);
            }
            catch (Exception ex)
            {
                LogError("Ошибка создания папок", ex);
            }
        }

        /// <summary>
        /// Создает изображение-заглушку
        /// </summary>
        private static void CreateDefaultImage(string imagesPath)
        {
            try
            {
                string defaultImagePath = Path.Combine(imagesPath, "picture.png");

                if (!File.Exists(defaultImagePath))
                {
                    using (Bitmap bitmap = new Bitmap(300, 200))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.Clear(Color.LightGray);

                            // Рисуем рамку
                            using (Pen pen = new Pen(Color.DarkGray, 2))
                            {
                                graphics.DrawRectangle(pen, 1, 1, 297, 197);
                            }

                            // Рисуем иконку фотоаппарата
                            using (Font font = new Font("Comic Sans MS", 48, FontStyle.Bold))
                            {
                                graphics.DrawString("📷", font, Brushes.Gray, 110, 50);
                            }

                            // Добавляем текст
                            using (Font font = new Font("Comic Sans MS", 14, FontStyle.Regular))
                            {
                                SizeF textSize = graphics.MeasureString("Нет изображения", font);
                                float x = (bitmap.Width - textSize.Width) / 2;
                                float y = 140;
                                graphics.DrawString("Нет изображения", font, Brushes.DarkGray, x, y);
                            }
                        }

                        bitmap.Save(defaultImagePath, ImageFormat.Png);
                        Console.WriteLine($"Создано изображение-заглушка: {defaultImagePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Ошибка создания изображения-заглушки", ex);
            }
        }

        /// <summary>
        /// Копирует логотип из ресурсов если он существует
        /// </summary>
        private static void CopyLogoIfExists(string resourcesPath)
        {
            try
            {
                string logoPath = Path.Combine(resourcesPath, "logo.png");

                if (!File.Exists(logoPath))
                {
                    // Проверяем различные возможные пути к логотипу
                    string[] possibleLogoPaths =
                    {
                        @"C:\Users\Lenovo\Downloads\Задание практика ПМ05\Практика ПМ 03-ПМ05\Задание на практику\Вариант 2\Общие ресурсы\logo.png",
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png"),
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "logo.png"),
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "logo.png")
                    };

                    foreach (string sourcePath in possibleLogoPaths)
                    {
                        if (File.Exists(sourcePath))
                        {
                            File.Copy(sourcePath, logoPath, true);
                            Console.WriteLine($"Скопирован логотип из: {sourcePath}");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("Ошибка копирования логотипа", ex);
            }
        }

        /// <summary>
        /// Проверяет подключение к базе данных
        /// </summary>
        private static bool CheckDatabaseConnection()
        {
            try
            {
                Console.WriteLine("Проверка подключения к базе данных...");

                bool connectionSuccess = DatabaseHelper.TestConnection();

                if (connectionSuccess)
                {
                    Console.WriteLine("✓ Подключение к базе данных успешно");
                    return true;
                }
                else
                {
                    Console.WriteLine("✗ Ошибка подключения к базе данных");
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogError("Ошибка проверки подключения к БД", ex);
                return false;
            }
        }

        /// <summary>
        /// Импортирует пользователей если таблица пустая
        /// </summary>
        private static void ImportUsers()
        {
            try
            {
                Console.WriteLine("Проверка наличия пользователей...");

                bool importNeeded = DatabaseHelper.ImportUsersIfNeeded();

                if (importNeeded)
                {
                    Console.WriteLine("✓ Пользователи импортированы");
                }
                else
                {
                    Console.WriteLine("✓ Пользователи уже существуют");
                }
            }
            catch (Exception ex)
            {
                LogError("Ошибка импорта пользователей", ex);
            }
        }

        /// <summary>
        /// Показывает загрузочный экран
        /// </summary>
        private static void ShowSplashScreen()
        {
            try
            {
                Form splashForm = new Form()
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.None,
                    Size = new Size(500, 300),
                    BackColor = Color.FromArgb(73, 140, 81), // Акцентный цвет из ТЗ
                    TopMost = true
                };

                // Панель для контента
                Panel contentPanel = new Panel()
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.White
                };

                // Логотип или иконка
                PictureBox logoPictureBox = new PictureBox()
                {
                    Size = new Size(100, 100),
                    Location = new Point(200, 30),
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                // Создаем простой логотип если файла нет
                Bitmap logoBitmap = new Bitmap(100, 100);
                using (Graphics g = Graphics.FromImage(logoBitmap))
                {
                    g.Clear(Color.FromArgb(118, 227, 131)); // Вторичный цвет из ТЗ
                    g.DrawString("🏋️",
                        new Font("Segoe UI Emoji", 48),
                        Brushes.White, 10, 20);
                }
                logoPictureBox.Image = logoBitmap;

                // Название приложения
                Label titleLabel = new Label()
                {
                    Text = "ООО СПОРТИВНЫЕ ТОВАРЫ",
                    Font = new Font("Comic Sans MS", 16, FontStyle.Bold),
                    ForeColor = Color.FromArgb(73, 140, 81),
                    AutoSize = true,
                    Location = new Point(120, 140)
                };

                // Подзаголовок
                Label subtitleLabel = new Label()
                {
                    Text = "Информационная система",
                    Font = new Font("Comic Sans MS", 12),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(160, 180)
                };

                // Индикатор загрузки
                ProgressBar progressBar = new ProgressBar()
                {
                    Location = new Point(100, 220),
                    Size = new Size(300, 20),
                    Style = ProgressBarStyle.Marquee,
                    MarqueeAnimationSpeed = 30
                };

                // Статус загрузки
                Label statusLabel = new Label()
                {
                    Text = "Загрузка приложения...",
                    Font = new Font("Comic Sans MS", 9),
                    ForeColor = Color.DarkGray,
                    AutoSize = true,
                    Location = new Point(200, 250)
                };

                // Добавляем элементы
                contentPanel.Controls.AddRange(new Control[]
                {
                    logoPictureBox, titleLabel, subtitleLabel,
                    progressBar, statusLabel
                });

                splashForm.Controls.Add(contentPanel);

                // Показываем форму
                splashForm.Show();
                Application.DoEvents();

                // Имитируем загрузку
                System.Threading.Thread.Sleep(2000);

                // Закрываем splash screen
                splashForm.Close();
                splashForm.Dispose();
            }
            catch (Exception ex)
            {
                LogError("Ошибка отображения splash screen", ex);
            }
        }

        /// <summary>
        /// Запускает основное приложение
        /// </summary>
        private static void RunApplication()
        {
            try
            {
                Console.WriteLine("Запуск приложения...");

                // Запускаем форму входа
                using (Form1 loginForm = new Form1())
                {
                    Application.Run(loginForm);
                }

                Console.WriteLine("Приложение завершено");
            }
            catch (Exception ex)
            {
                LogError("Критическая ошибка при запуске приложения", ex);
                ShowFatalError(ex);
            }
        }

        /// <summary>
        /// Показывает ошибку подключения к БД
        /// </summary>
        private static void ShowDatabaseError()
        {
            string errorMessage =
                "Не удалось подключиться к базе данных!\n\n" +
                "Возможные причины:\n" +
                "1. SQL Server LocalDB не установлен\n" +
                "2. База данных SportsGoodsDB_Cyrillic не существует\n" +
                "3. Сервер (localdb)\\MSSQLLocalDB не запущен\n\n" +
                "Для устранения проблемы:\n" +
                "1. Установите SQL Server LocalDB\n" +
                "2. Запустите скрипт создания базы данных\n" +
                "3. Проверьте строку подключения в настройках";

            MessageBox.Show(errorMessage,
                "Ошибка подключения к базе данных",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// Показывает фатальную ошибку
        /// </summary>
        private static void ShowFatalError(Exception ex)
        {
            string errorDetails = $"Тип ошибки: {ex.GetType().Name}\n" +
                                $"Сообщение: {ex.Message}\n\n" +
                                $"StackTrace:\n{ex.StackTrace}";

            string userMessage =
                "Произошла критическая ошибка в приложении!\n\n" +
                "Приложение будет закрыто.\n" +
                "Пожалуйста, сообщите об ошибке администратору.\n\n" +
                $"Детали ошибки:\n{ex.Message}";

            // Логируем полную ошибку
            LogError("ФАТАЛЬНАЯ ОШИБКА", ex);

            // Показываем пользователю упрощенное сообщение
            MessageBox.Show(userMessage,
                "Критическая ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>
        /// Логирует ошибку в файл и консоль
        /// </summary>
        private static void LogError(string message, Exception ex = null)
        {
            try
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

                if (ex != null)
                {
                    logMessage += $"\nException: {ex.GetType().Name}\n" +
                                 $"Message: {ex.Message}\n" +
                                 $"StackTrace: {ex.StackTrace}\n";
                }

                // Выводим в консоль
                Console.WriteLine(logMessage);

                // Записываем в файл
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                string logFile = Path.Combine(logDirectory, $"error_{DateTime.Now:yyyy-MM-dd}.log");
                File.AppendAllText(logFile, logMessage + "\n\n");
            }
            catch
            {
                // Если не удалось записать лог, игнорируем
            }
        }

        /// <summary>
        /// Проверяет и создает базу данных если её нет
        /// </summary>
        private static void CheckAndCreateDatabase()
        {
            try
            {
                // Проверяем существование основных таблиц
                string[] requiredTables = { "Users", "Products", "Orders", "PickupPoints" };

                foreach (string table in requiredTables)
                {
                    if (!DatabaseHelper.CheckTableExists(table))
                    {
                        MessageBox.Show($"Таблица {table} не найдена в базе данных.\n" +
                                      "Выполните скрипт создания базы данных.",
                                      "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                Console.WriteLine("✓ Все необходимые таблицы существуют");
            }
            catch (Exception ex)
            {
                LogError("Ошибка проверки структуры БД", ex);
            }
        }
    }
}