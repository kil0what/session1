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

        // Проверяем существование столбцов
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