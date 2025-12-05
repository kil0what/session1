using System;
using System.Collections.Generic;

namespace SportsGoodsApp
{
    // Класс User с полным набором свойств
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }

    // Класс Product с полным набором свойств
    public class Product
    {
        public string ArticleNumber { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; } // Единица измерения
        public decimal Price { get; set; }
        public decimal MaxDiscount { get; set; }
        public string Manufacturer { get; set; }
        public string Supplier { get; set; }
        public string Category { get; set; }
        public decimal CurrentDiscount { get; set; }
        public int StockQuantity { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }

    // Класс Order для заказов
    public class Order
    {
        public int Id { get; set; }
        public string OrderItems { get; set; } // Формат: "Артикул1,количество1,Артикул2,количество2"
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int PickupPointId { get; set; }
        public string CustomerName { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
    }

    // Класс PickupPoint для пунктов выдачи
    public class PickupPoint
    {
        public int Id { get; set; }
        public string Address { get; set; }
    }

    // Класс Session для хранения текущего пользователя
    public static class Session
    {
        public static User CurrentUser { get; set; }
        public static bool IsAdmin => CurrentUser?.Role == "Администратор";
        public static bool IsManager => CurrentUser?.Role == "Менеджер";
        public static bool IsClient => CurrentUser?.Role == "Клиент";
        public static bool IsGuest => CurrentUser?.Role == "Guest";
    }

    // Класс FakeDatabase с тестовыми данными
    public static class FakeDatabase
    {
        public static List<User> Users = new List<User>
        {
            new User {
                Id = 1,
                Login = "m4ic8j5qgstw@gmail.com",
                Password = "2L6KZG",
                FullName = "Пахомова Аиша Анатольевна",
                Role = "Admin",
                Email = "m4ic8j5qgstw@gmail.com"
            },
            new User {
                Id = 2,
                Login = "d43zfg9tlsyv@gmail.com",
                Password = "uzWC67",
                FullName = "Жуков Роман Богданович",
                Role = "Admin",
                Email = "d43zfg9tlsyv@gmail.com"
            },
            new User {
                Id = 3,
                Login = "8ohgisf6k45w@outlook.com",
                Password = "8ntwUp",
                FullName = "Киселева Анастасия Максимовна",
                Role = "Admin",
                Email = "8ohgisf6k45w@outlook.com"
            },
            new User {
                Id = 4,
                Login = "hi1brwj46czx@mail.com",
                Password = "YOYhfR",
                FullName = "Григорьева Арина Арсентьевна",
                Role = "Manager",
                Email = "hi1brwj46czx@mail.com"
            },
            new User {
                Id = 5,
                Login = "fvkbcamhlj52@gmail.com",
                Password = "RSbvHv",
                FullName = "Иванов Лев Михайлович",
                Role = "Manager",
                Email = "fvkbcamhlj52@gmail.com"
            },
            new User {
                Id = 6,
                Login = "9qxnce8jwruv@gmail.com",
                Password = "rwVDh9",
                FullName = "Григорьев Лев Давидович",
                Role = "Manager",
                Email = "9qxnce8jwruv@gmail.com"
            },
            new User {
                Id = 7,
                Login = "dotiex942p1r@gmail.com",
                Password = "LdNyos",
                FullName = "Поляков Степан Егорович",
                Role = "Client",
                Email = "dotiex942p1r@gmail.com"
            },
            new User {
                Id = 8,
                Login = "n0bmi2h1xral@tutanota.com",
                Password = "gynQMT",
                FullName = "Леонова Алиса Кирилловна",
                Role = "Client",
                Email = "n0bmi2h1xral@tutanota.com"
            },
            new User {
                Id = 9,
                Login = "sfm3t278kdvz@yahoo.com",
                Password = "AtnDjr",
                FullName = "Яковлев Платон Константинович",
                Role = "Client",
                Email = "sfm3t278kdvz@yahoo.com"
            },
            new User {
                Id = 10,
                Login = "ilb8rdut0v7e@mail.com",
                Password = "JlFRCZ",
                FullName = "Ковалева Ева Яковлевна",
                Role = "Client",
                Email = "ilb8rdut0v7e@mail.com"
            }
        };

        public static List<Product> Products = new List<Product>
        {
            new Product
            {
                ArticleNumber = "А112Т4",
                Name = "Боксерская груша",
                Unit = "шт.",
                Price = 778,
                MaxDiscount = 30,
                Manufacturer = "X-Match",
                Supplier = "Спортмастер",
                Category = "Спортивный инвентарь",
                CurrentDiscount = 5,
                StockQuantity = 6,
                Description = "Боксерская груша X-Match черная",
                ImagePath = "А112Т4.jpg"
            },
            new Product
            {
                ArticleNumber = "G598Y6",
                Name = "Спортивный мат",
                Unit = "шт.",
                Price = 2390,
                MaxDiscount = 15,
                Manufacturer = "Perfetto Sport",
                Supplier = "Декатлон",
                Category = "Спортивный инвентарь",
                CurrentDiscount = 2,
                StockQuantity = 16,
                Description = "Спортивный мат 100x100x10 см Perfetto Sport № 3 бежевый",
                ImagePath = "G598Y6.jpg"
            },
            // Добавьте остальные товары из Excel файла
        };

        public static List<PickupPoint> PickupPoints = new List<PickupPoint>
        {
            new PickupPoint { Id = 1, Address = "344288, г. Дубна, ул. Чехова, 1" },
            new PickupPoint { Id = 2, Address = "614164, г.Дубна, ул. Степная, 30" },
            // Добавьте остальные адреса
        };

        public static List<Order> Orders = new List<Order>
        {
            new Order
            {
                Id = 1,
                OrderItems = "А112Т4,2,G598Y6,2",
                OrderDate = new DateTime(2022, 5, 15),
                DeliveryDate = new DateTime(2022, 5, 21),
                PickupPointId = 18,
                CustomerName = "Поляков Степан Егорович",
                Code = "401",
                Status = "Новый"
            },
            // Добавьте остальные заказы
        };
    }
}