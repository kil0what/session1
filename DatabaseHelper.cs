using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace SportsGoodsApp
{
    public class DatabaseHelper
    {
        public static string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SportsGoodsDB_Cyrillic;Integrated Security=True;Connect Timeout=3;";

        private static bool CheckColumnExists(string tableName, string columnName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_NAME = @TableName AND COLUMN_NAME = @ColumnName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TableName", tableName);
                        command.Parameters.AddWithValue("@ColumnName", columnName);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
public static bool ImportUsersFromExcel()
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
                    MessageBox.Show($"В базе уже есть {existingUsers} пользователей. Импорт не требуется.",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
            }
            
            // SQL для добавления пользователей
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
                MessageBox.Show($"Импортировано {rowsAffected} пользователей", 
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        // 1. Асинхронная аутентификация
        public static async Task<User> AuthenticateUserAsync(string login, string password)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    string query = @"
                        SELECT Id, Login, FullName, Role, Email 
                        FROM Users 
                        WHERE Login = @Login AND Password = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", password);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            if (await reader.ReadAsync().ConfigureAwait(false))
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
            catch (SqlException ex) when (ex.Number == -2)
            {
                MessageBox.Show("Таймаут подключения к базе данных. Попробуйте снова.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        // 2. Асинхронное получение товаров (с проверкой столбцов)
        public static async Task<List<Product>> GetAllProductsAsync()
        {
            List<Product> products = new List<Product>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    // Динамически формируем запрос в зависимости от существующих столбцов
                    string query = @"
                        SELECT ArticleNumber, Name, Unit, Price, StockQuantity, Description, ImagePath";

                    // Добавляем столбцы если они существуют
                    if (CheckColumnExists("Products", "Category"))
                        query += ", Category";
                    if (CheckColumnExists("Products", "Manufacturer"))
                        query += ", Manufacturer";
                    if (CheckColumnExists("Products", "Supplier"))
                        query += ", Supplier";
                    if (CheckColumnExists("Products", "MaxDiscount"))
                        query += ", MaxDiscount";
                    if (CheckColumnExists("Products", "CurrentDiscount"))
                        query += ", CurrentDiscount";

                    query += " FROM Products ORDER BY Name";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
                        {
                            Product product = new Product
                            {
                                ArticleNumber = reader["ArticleNumber"].ToString(),
                                Name = reader["Name"].ToString(),
                                Unit = reader["Unit"] != DBNull.Value ? reader["Unit"].ToString() : "шт.",
                                Price = Convert.ToDecimal(reader["Price"]),
                                StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                ImagePath = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : null
                            };

                            // Заполняем опциональные поля если они есть
                            try { product.Category = reader["Category"] != DBNull.Value ? reader["Category"].ToString() : null; } catch { }
                            try { product.Manufacturer = reader["Manufacturer"] != DBNull.Value ? reader["Manufacturer"].ToString() : null; } catch { }
                            try { product.Supplier = reader["Supplier"] != DBNull.Value ? reader["Supplier"].ToString() : null; } catch { }
                            try { product.MaxDiscount = reader["MaxDiscount"] != DBNull.Value ? Convert.ToDecimal(reader["MaxDiscount"]) : 0; } catch { }
                            try { product.CurrentDiscount = reader["CurrentDiscount"] != DBNull.Value ? Convert.ToDecimal(reader["CurrentDiscount"]) : 0; } catch { }

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

        // 3. Асинхронное получение товара по артикулу
        public static async Task<Product> GetProductByArticleAsync(string articleNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    string query = "SELECT * FROM Products WHERE ArticleNumber = @ArticleNumber";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ArticleNumber", articleNumber);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            if (await reader.ReadAsync().ConfigureAwait(false))
                            {
                                Product product = new Product
                                {
                                    ArticleNumber = reader["ArticleNumber"].ToString(),
                                    Name = reader["Name"].ToString(),
                                    Unit = reader["Unit"] != DBNull.Value ? reader["Unit"].ToString() : "шт.",
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                                    Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                    ImagePath = reader["ImagePath"] != DBNull.Value ? reader["ImagePath"].ToString() : null
                                };

                                // Опциональные поля
                                try { product.MaxDiscount = reader["MaxDiscount"] != DBNull.Value ? Convert.ToDecimal(reader["MaxDiscount"]) : 0; } catch { }
                                try { product.Manufacturer = reader["Manufacturer"] != DBNull.Value ? reader["Manufacturer"].ToString() : null; } catch { }
                                try { product.Supplier = reader["Supplier"] != DBNull.Value ? reader["Supplier"].ToString() : null; } catch { }
                                try { product.Category = reader["Category"] != DBNull.Value ? reader["Category"].ToString() : null; } catch { }
                                try { product.CurrentDiscount = reader["CurrentDiscount"] != DBNull.Value ? Convert.ToDecimal(reader["CurrentDiscount"]) : 0; } catch { }

                                return product;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения товара: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        // 4. Добавление товара
        public static async Task<bool> AddProductAsync(Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    // Формируем запрос динамически
                    string columns = "ArticleNumber, Name, Unit, Price, StockQuantity";
                    string values = "@ArticleNumber, @Name, @Unit, @Price, @StockQuantity";

                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@ArticleNumber", product.ArticleNumber),
                        new SqlParameter("@Name", product.Name),
                        new SqlParameter("@Unit", product.Unit ?? "шт."),
                        new SqlParameter("@Price", product.Price),
                        new SqlParameter("@StockQuantity", product.StockQuantity)
                    };

                    if (!string.IsNullOrEmpty(product.Description) && CheckColumnExists("Products", "Description"))
                    {
                        columns += ", Description";
                        values += ", @Description";
                        parameters.Add(new SqlParameter("@Description", product.Description));
                    }

                    if (!string.IsNullOrEmpty(product.ImagePath) && CheckColumnExists("Products", "ImagePath"))
                    {
                        columns += ", ImagePath";
                        values += ", @ImagePath";
                        parameters.Add(new SqlParameter("@ImagePath", product.ImagePath));
                    }

                    if (!string.IsNullOrEmpty(product.Category) && CheckColumnExists("Products", "Category"))
                    {
                        columns += ", Category";
                        values += ", @Category";
                        parameters.Add(new SqlParameter("@Category", product.Category));
                    }

                    if (!string.IsNullOrEmpty(product.Manufacturer) && CheckColumnExists("Products", "Manufacturer"))
                    {
                        columns += ", Manufacturer";
                        values += ", @Manufacturer";
                        parameters.Add(new SqlParameter("@Manufacturer", product.Manufacturer));
                    }

                    string query = $"INSERT INTO Products ({columns}) VALUES ({values})";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters.ToArray());

                        int rowsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                MessageBox.Show($"Товар с артикулом '{product.ArticleNumber}' уже существует!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления товара: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        // 5. Обновление товара
        public static async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    // Динамически формируем SET часть
                    var setParts = new List<string>();
                    var parameters = new List<SqlParameter>();

                    setParts.Add("Name = @Name");
                    parameters.Add(new SqlParameter("@Name", product.Name));

                    setParts.Add("Unit = @Unit");
                    parameters.Add(new SqlParameter("@Unit", product.Unit ?? "шт."));

                    setParts.Add("Price = @Price");
                    parameters.Add(new SqlParameter("@Price", product.Price));

                    setParts.Add("StockQuantity = @StockQuantity");
                    parameters.Add(new SqlParameter("@StockQuantity", product.StockQuantity));

                    if (CheckColumnExists("Products", "Description"))
                    {
                        setParts.Add("Description = @Description");
                        parameters.Add(new SqlParameter("@Description", (object)product.Description ?? DBNull.Value));
                    }

                    if (CheckColumnExists("Products", "ImagePath"))
                    {
                        setParts.Add("ImagePath = @ImagePath");
                        parameters.Add(new SqlParameter("@ImagePath", (object)product.ImagePath ?? DBNull.Value));
                    }

                    if (CheckColumnExists("Products", "Category"))
                    {
                        setParts.Add("Category = @Category");
                        parameters.Add(new SqlParameter("@Category", (object)product.Category ?? DBNull.Value));
                    }

                    if (CheckColumnExists("Products", "Manufacturer"))
                    {
                        setParts.Add("Manufacturer = @Manufacturer");
                        parameters.Add(new SqlParameter("@Manufacturer", (object)product.Manufacturer ?? DBNull.Value));
                    }

                    string setClause = string.Join(", ", setParts);
                    string query = $"UPDATE Products SET {setClause} WHERE ArticleNumber = @ArticleNumber";

                    parameters.Add(new SqlParameter("@ArticleNumber", product.ArticleNumber));

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters.ToArray());

                        int rowsAffected = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления товара: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        // 6. Синхронные обертки для совместимости
        public static User AuthenticateUser(string login, string password)
        {
            try
            {
                return Task.Run(() => AuthenticateUserAsync(login, password)).GetAwaiter().GetResult();
            }
            catch
            {
                return null;
            }
        }

        public static List<Product> GetAllProducts()
        {
            try
            {
                return Task.Run(() => GetAllProductsAsync()).GetAwaiter().GetResult();
            }
            catch
            {
                return new List<Product>();
            }
        }

        public static bool AddProduct(Product product)
        {
            try
            {
                return Task.Run(() => AddProductAsync(product)).GetAwaiter().GetResult();
            }
            catch
            {
                return false;
            }
        }

        public static bool UpdateProduct(Product product)
        {
            try
            {
                return Task.Run(() => UpdateProductAsync(product)).GetAwaiter().GetResult();
            }
            catch
            {
                return false;
            }
        }

        // 7. Проверка подключения
        public static async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        // 8. Получение производителей
        public static async Task<List<string>> GetManufacturersAsync()
        {
            List<string> manufacturers = new List<string>();

            if (!CheckColumnExists("Products", "Manufacturer"))
                return manufacturers;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    string query = @"
                        SELECT DISTINCT Manufacturer 
                        FROM Products 
                        WHERE Manufacturer IS NOT NULL AND Manufacturer != ''
                        ORDER BY Manufacturer";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
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

        // 9. Получение категорий
        public static async Task<List<string>> GetCategoriesAsync()
        {
            List<string> categories = new List<string>();

            if (!CheckColumnExists("Products", "Category"))
                return categories;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    string query = @"
                        SELECT DISTINCT Category 
                        FROM Products 
                        WHERE Category IS NOT NULL AND Category != ''
                        ORDER BY Category";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        while (await reader.ReadAsync().ConfigureAwait(false))
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
        public static List<string> GetManufacturers()
        {
            try
            {
                return Task.Run(() => GetManufacturersAsync()).GetAwaiter().GetResult();
            }
            catch
            {
                return new List<string>();
            }
        }

        public static List<string> GetCategories()
        {
            try
            {
                return Task.Run(() => GetCategoriesAsync()).GetAwaiter().GetResult();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}