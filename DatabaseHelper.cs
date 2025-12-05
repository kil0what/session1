using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportsGoodsApp
{
    public class DatabaseHelper
    {
        // Строка подключения к LocalDB
        public static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SportsGoodsDB_Cyrillic;Integrated Security=True;";

        // Проверка существования таблицы
        public static bool CheckTableExists(string tableName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = @TableName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TableName", tableName);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // Проверка подключения к БД
        // Проверка подключения (синхронная версия)
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Проверяем существование таблицы Users
                    string query = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users'";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        int tableCount = Convert.ToInt32(cmd.ExecuteScalar());
                        if (tableCount == 0)
                        {
                            MessageBox.Show("Таблица Users не найдена. Выполните скрипт создания базы данных.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

                    return true;
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Ошибка SQL: {sqlEx.Message}\nНомер ошибки: {sqlEx.Number}",
                    "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось подключиться к базе данных:\n{ex.Message}\n\n" +
                    "Убедитесь, что:\n" +
                    "1. Установлен SQL Server LocalDB\n" +
                    "2. База SportsGoodsDB_Cyrillic существует\n" +
                    "3. Сервер (localdb)\\MSSQLLocalDB запущен",
                    "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Асинхронная версия (если нужна для других целей)
        public static async Task<bool> TestConnectionAsync()
        {
            return await Task.Run(() => TestConnection());
        }
        // Импорт пользователей если таблица пустая
        public static bool ImportUsersIfNeeded()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Проверяем, есть ли уже пользователи
                    string checkQuery = "SELECT COUNT(*) FROM Users";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        int existingUsers = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (existingUsers > 0)
                        {
                            return true; // Пользователи уже есть
                        }
                    }

                    // Добавляем пользователей
                    string insertQuery = @"
                        INSERT INTO Users (Login, Password, FullName, Role, Email) VALUES
                        (@Login1, @Password1, @FullName1, @Role1, @Email1),
                        (@Login2, @Password2, @FullName2, @Role2, @Email2),
                        (@Login3, @Password3, @FullName3, @Role3, @Email3),
                        (@Login4, @Password4, @FullName4, @Role4, @Email4),
                        (@Login5, @Password5, @FullName5, @Role5, @Email5),
                        (@Login6, @Password6, @FullName6, @Role6, @Email6),
                        (@Login7, @Password7, @FullName7, @Role7, @Email7),
                        (@Login8, @Password8, @FullName8, @Role8, @Email8),
                        (@Login9, @Password9, @FullName9, @Role9, @Email9),
                        (@Login10, @Password10, @FullName10, @Role10, @Email10)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        // Пользователь 1
                        command.Parameters.AddWithValue("@Login1", "m4ic8j5qgstw@gmail.com");
                        command.Parameters.AddWithValue("@Password1", "2L6KZG");
                        command.Parameters.AddWithValue("@FullName1", "Пахомова Аиша Анатольевна");
                        command.Parameters.AddWithValue("@Role1", "Администратор");
                        command.Parameters.AddWithValue("@Email1", "m4ic8j5qgstw@gmail.com");

                        // Пользователь 2
                        command.Parameters.AddWithValue("@Login2", "d43zfg9tlsyv@gmail.com");
                        command.Parameters.AddWithValue("@Password2", "uzWC67");
                        command.Parameters.AddWithValue("@FullName2", "Жуков Роман Богданович");
                        command.Parameters.AddWithValue("@Role2", "Администратор");
                        command.Parameters.AddWithValue("@Email2", "d43zfg9tlsyv@gmail.com");

                        // Пользователь 3
                        command.Parameters.AddWithValue("@Login3", "8ohgisf6k45w@outlook.com");
                        command.Parameters.AddWithValue("@Password3", "8ntwUp");
                        command.Parameters.AddWithValue("@FullName3", "Киселева Анастасия Максимовна");
                        command.Parameters.AddWithValue("@Role3", "Администратор");
                        command.Parameters.AddWithValue("@Email3", "8ohgisf6k45w@outlook.com");

                        // Пользователь 4
                        command.Parameters.AddWithValue("@Login4", "hi1brwj46czx@mail.com");
                        command.Parameters.AddWithValue("@Password4", "YOYhfR");
                        command.Parameters.AddWithValue("@FullName4", "Григорьева Арина Арсентьевна");
                        command.Parameters.AddWithValue("@Role4", "Менеджер");
                        command.Parameters.AddWithValue("@Email4", "hi1brwj46czx@mail.com");

                        // Пользователь 5
                        command.Parameters.AddWithValue("@Login5", "fvkbcamhlj52@gmail.com");
                        command.Parameters.AddWithValue("@Password5", "RSbvHv");
                        command.Parameters.AddWithValue("@FullName5", "Иванов Лев Михайлович");
                        command.Parameters.AddWithValue("@Role5", "Менеджер");
                        command.Parameters.AddWithValue("@Email5", "fvkbcamhlj52@gmail.com");

                        // Пользователь 6
                        command.Parameters.AddWithValue("@Login6", "9qxnce8jwruv@gmail.com");
                        command.Parameters.AddWithValue("@Password6", "rwVDh9");
                        command.Parameters.AddWithValue("@FullName6", "Григорьев Лев Давидович");
                        command.Parameters.AddWithValue("@Role6", "Менеджер");
                        command.Parameters.AddWithValue("@Email6", "9qxnce8jwruv@gmail.com");

                        // Пользователь 7
                        command.Parameters.AddWithValue("@Login7", "dotiex942p1r@gmail.com");
                        command.Parameters.AddWithValue("@Password7", "LdNyos");
                        command.Parameters.AddWithValue("@FullName7", "Поляков Степан Егорович");
                        command.Parameters.AddWithValue("@Role7", "Клиент");
                        command.Parameters.AddWithValue("@Email7", "dotiex942p1r@gmail.com");

                        // Пользователь 8
                        command.Parameters.AddWithValue("@Login8", "n0bmi2h1xral@tutanota.com");
                        command.Parameters.AddWithValue("@Password8", "gynQMT");
                        command.Parameters.AddWithValue("@FullName8", "Леонова Алиса Кирилловна");
                        command.Parameters.AddWithValue("@Role8", "Клиент");
                        command.Parameters.AddWithValue("@Email8", "n0bmi2h1xral@tutanota.com");

                        // Пользователь 9
                        command.Parameters.AddWithValue("@Login9", "sfm3t278kdvz@yahoo.com");
                        command.Parameters.AddWithValue("@Password9", "AtnDjr");
                        command.Parameters.AddWithValue("@FullName9", "Яковлев Платон Константинович");
                        command.Parameters.AddWithValue("@Role9", "Клиент");
                        command.Parameters.AddWithValue("@Email9", "sfm3t278kdvz@yahoo.com");

                        // Пользователь 10
                        command.Parameters.AddWithValue("@Login10", "ilb8rdut0v7e@mail.com");
                        command.Parameters.AddWithValue("@Password10", "JlFRCZ");
                        command.Parameters.AddWithValue("@FullName10", "Ковалева Ева Яковлевна");
                        command.Parameters.AddWithValue("@Role10", "Клиент");
                        command.Parameters.AddWithValue("@Email10", "ilb8rdut0v7e@mail.com");

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка импорта пользователей: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Аутентификация пользователя
        public static User AuthenticateUser(string login, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT Id, Login, FullName, Role, Email 
                        FROM Users 
                        WHERE Login = @Login AND Password = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Login = reader["Login"].ToString(),
                                    FullName = reader["FullName"].ToString(),
                                    Role = reader["Role"].ToString(),
                                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        // Получение всех товаров
        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT ArticleNumber, Name, Unit, Price, MaxDiscount, Manufacturer, 
                               Supplier, Category, CurrentDiscount, StockQuantity, Description, ImagePath
                        FROM Products 
                        ORDER BY Name";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                ArticleNumber = reader["ArticleNumber"].ToString(),
                                Name = reader["Name"].ToString(),
                                Unit = reader["Unit"] != DBNull.Value ? reader["Unit"].ToString() : "шт.",
                                Price = Convert.ToDecimal(reader["Price"]),
                                MaxDiscount = reader["MaxDiscount"] != DBNull.Value ? Convert.ToDecimal(reader["MaxDiscount"]) : 0,
                                Manufacturer = reader["Manufacturer"] != DBNull.Value ? reader["Manufacturer"].ToString() : null,
                                Supplier = reader["Supplier"] != DBNull.Value ? reader["Supplier"].ToString() : null,
                                Category = reader["Category"] != DBNull.Value ? reader["Category"].ToString() : null,
                                CurrentDiscount = reader["CurrentDiscount"] != DBNull.Value ? Convert.ToDecimal(reader["CurrentDiscount"]) : 0,
                                StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                ImagePath = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : null
                            };

                            products.Add(product);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return products;
        }

        // Добавление товара
        public static bool AddProduct(Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO Products (ArticleNumber, Name, Unit, Price, MaxDiscount, Manufacturer, 
                                              Supplier, Category, CurrentDiscount, StockQuantity, Description, ImagePath)
                        VALUES (@ArticleNumber, @Name, @Unit, @Price, @MaxDiscount, @Manufacturer, 
                                @Supplier, @Category, @CurrentDiscount, @StockQuantity, @Description, @ImagePath)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ArticleNumber", product.ArticleNumber);
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@Unit", product.Unit ?? "шт.");
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@MaxDiscount", product.MaxDiscount);
                        command.Parameters.AddWithValue("@Manufacturer", (object)product.Manufacturer ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Supplier", (object)product.Supplier ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Category", (object)product.Category ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CurrentDiscount", product.CurrentDiscount);
                        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                        command.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);
                        command.Parameters.AddWithValue("@ImagePath", (object)product.ImagePath ?? DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627) // Ошибка дублирования ключа
            {
                MessageBox.Show($"Товар с артикулом '{product.ArticleNumber}' уже существует!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления товара: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Обновление товара
        public static bool UpdateProduct(Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        UPDATE Products 
                        SET Name = @Name, Unit = @Unit, Price = @Price, MaxDiscount = @MaxDiscount,
                            Manufacturer = @Manufacturer, Supplier = @Supplier, Category = @Category,
                            CurrentDiscount = @CurrentDiscount, StockQuantity = @StockQuantity,
                            Description = @Description, ImagePath = @ImagePath
                        WHERE ArticleNumber = @ArticleNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ArticleNumber", product.ArticleNumber);
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@Unit", product.Unit ?? "шт.");
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@MaxDiscount", product.MaxDiscount);
                        command.Parameters.AddWithValue("@Manufacturer", (object)product.Manufacturer ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Supplier", (object)product.Supplier ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Category", (object)product.Category ?? DBNull.Value);
                        command.Parameters.AddWithValue("@CurrentDiscount", product.CurrentDiscount);
                        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                        command.Parameters.AddWithValue("@Description", (object)product.Description ?? DBNull.Value);
                        command.Parameters.AddWithValue("@ImagePath", (object)product.ImagePath ?? DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления товара: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Удаление товара
        public static bool DeleteProduct(string articleNumber)
        {
            try
            {
                // Проверяем, есть ли товар в заказах
                if (IsProductInOrder(articleNumber))
                {
                    MessageBox.Show("Товар присутствует в заказе и не может быть удален.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM Products WHERE ArticleNumber = @ArticleNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ArticleNumber", articleNumber);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления товара: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Проверка, есть ли товар в заказах
        private static bool IsProductInOrder(string articleNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT COUNT(*) 
                        FROM Orders 
                        WHERE OrderItems LIKE '%' + @ArticleNumber + '%'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ArticleNumber", articleNumber);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        // Получение производителей
        public static List<string> GetManufacturers()
        {
            List<string> manufacturers = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT DISTINCT Manufacturer 
                        FROM Products 
                        WHERE Manufacturer IS NOT NULL AND Manufacturer != ''
                        ORDER BY Manufacturer";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            manufacturers.Add(reader["Manufacturer"].ToString());
                        }
                    }
                }
            }
            catch
            {
                // Игнорируем ошибку
            }

            return manufacturers;
        }

        // Получение категорий
        public static List<string> GetCategories()
        {
            List<string> categories = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                        SELECT DISTINCT Category 
                        FROM Products 
                        WHERE Category IS NOT NULL AND Category != ''
                        ORDER BY Category";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(reader["Category"].ToString());
                        }
                    }
                }
            }
            catch
            {
                // Игнорируем ошибку
            }

            return categories;
        }
    }
}