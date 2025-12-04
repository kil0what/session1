USE SportsGoodsDB;
GO

-- 1. Вставляем пользователей из user_import.xlsx
INSERT INTO Users (Login, Password, FullName, Role) VALUES
('m4ic8j5qgstw@gmail.com', '2L6KZG', 'Пахомова Аиша Анатольевна', 'Admin'),
('d43zfg9tlsyv@gmail.com', 'uzWC67', 'Жуков Роман Богданович', 'Admin'),
('8ohgisf6k45w@outlook.com', '8ntwUp', 'Киселева Анастасия Максимовна', 'Admin'),
('hi1brwj46czx@mail.com', 'Y0yhfR', 'Григорьева Арина Арсентьевна', 'Manager'),
('fvkbcamhlj52@gmail.com', 'RSbvHv', 'Иванов Лев Михайлович', 'Manager'),
('9qxnce8jwruv@gmail.com', 'rwVDh9', 'Григорьев Лев Давидович', 'Manager'),
('dotiex942p1r@gmail.com', 'LdNyos', 'Поляков Степан Егорович', 'Client'),
('n0bmi2h1xral@tutanota.com', 'gynQMT', 'Леонова Алиса Кирилловна', 'Client'),
('sfm3t278kdvz@yahoo.com', 'AtnDjr', 'Яковлев Платон Константинович', 'Client'),
('ilb8rdut0v7e@mail.com', 'JlFRCZ', 'Ковалева Ева Яковлевна', 'Client');
GO

-- 2. Вставляем пункты выдачи из Пункты выдачи_import.xlsx
INSERT INTO DeliveryPoints (Address) VALUES
('344288, г. Дубна, ул. Чехова, 1'),
('614164, г.Дубна, ул. Степная, 30'),
('394242, г. Дубна, ул. Коммунистическая, 43'),
('660540, г. Дубна, ул. Солнечная, 25'),
('125837, г. Дубна, ул. Шоссейная, 40'),
('125703, г. Дубна, ул. Партизанская, 49'),
('625283, г. Дубна, ул. Победы, 46'),
('614611, г. Дубна, ул. Молодежная, 50'),
('454311, г.Дубна, ул. Новая, 19'),
('660007, г.Дубна, ул. Октябрьская, 19'),
('603036, г. Дубна, ул. Садовая, 4'),
('450983, г.Дубна, ул. Комсомольская, 26'),
('394782, г. Дубна, ул. Чехова, 3'),
('603002, г. Дубна, ул. Дзержинского, 28'),
('450558, г. Дубна, ул. Набережная, 30'),
('394060, г.Дубна, ул. Фрунзе, 43'),
('410661, г. Дубна, ул. Школьная, 50'),
('625590, г. Дубна, ул. Коммунистическая, 20'),
('625683, г. Дубна, ул. 8 Марта'),
('400562, г. Дубна, ул. Зеленая, 32'),
('614510, г. Дубна, ул. Маяковского, 47'),
('410542, г. Дубна, ул. Светлая, 46'),
('620839, г. Дубна, ул. Цветочная, 8'),
('443890, г. Дубна, ул. Коммунистическая, 1'),
('603379, г. Дубна, ул. Спортивная, 46'),
('603721, г. Дубна, ул. Гоголя, 41'),
('410172, г. Дубна, ул. Северная, 13'),
('420151, г. Дубна, ул. Вишневая, 32'),
('125061, г. Дубна, ул. Подгорная, 8'),
('630370, г. Дубна, ул. Шоссейная, 24'),
('614753, г. Дубна, ул. Полевая, 35'),
('426030, г. Дубна, ул. Маяковского, 44'),
('450375, г. Дубна ул. Клубная, 44'),
('625560, г. Дубна, ул. Некрасова, 12'),
('630201, г. Дубна, ул. Комсомольская, 17'),
('190949, г. Дубна, ул. Мичурина, 26');
GO

-- 3. Вставляем категории товаров
INSERT INTO Categories (Name) VALUES
('Спортивный инвентарь'),
('Одежда');
GO

-- 4. Вставляем производителей
INSERT INTO Manufacturers (Name) VALUES
('X-Match'),
('Perfetto Sport'),
('ROMANA Next'),
('Moby Kids'),
('playToday'),
('Совтехстром'),
('Abtoys'),
('DFC'),
('Nordway'),
('Ridex'),
('Salomon'),
('Mikasa'),
('Molten'),
('Colton'),
('Atemi'),
('Green Hill'),
('SKIF'),
('Starfit'),
('Bradex'),
('Demix');
GO

-- 5. Вставляем поставщиков
INSERT INTO Suppliers (Name) VALUES
('Спортмастер'),
('Декатлон');
GO

-- 6. Вставляем товары из Товар_import_Спортивные товары.xlsx
INSERT INTO Products (ArticleNumber, Name, Unit, Price, MaxDiscount, ManufacturerId, SupplierId, CategoryId, CurrentDiscount, StockQuantity, Description, ImagePath) VALUES
('А112Т4', 'Боксерская груша', 'шт.', 778.00, 30.00, 1, 1, 1, 5.00, 6, 'Боксерская груша X-Match черная', 'А112Т4.jpg'),
('G598Y6', 'Спортивный мат', 'шт.', 2390.00, 15.00, 2, 2, 1, 2.00, 16, 'Спортивный мат 100x100x10 см Perfetto Sport № 3 бежевый', 'G598Y6.jpg'),
('F746E6', 'Шведская стенка', 'шт.', 9900.00, 10.00, 3, 2, 1, 3.00, 5, 'Шведская стенка ROMANA Next, pastel', 'F746E6.jpg'),
('D830R5', 'Тренажер прыжков', 'шт.', 1120.00, 15.00, 4, 1, 1, 4.00, 8, 'Тренажер для прыжков Moby Kids Moby-Jumper со счетчиком', 'D830R5.jpg'),
('B538G6', 'Спортивный костюм', 'шт.', 839.00, 5.00, 5, 1, 2, 3.00, 17, 'Спортивный костюм playToday (футболка + шорты)', 'B538G6.jpg'),
('D648N7', 'Набор для хоккея', 'шт.', 350.00, 10.00, 6, 2, 1, 4.00, 7, 'Набор для хоккея Совтехстром', 'D648N7.jpg'),
('F735B6', 'Игровой набор', 'шт.', 320.00, 15.00, 6, 2, 1, 2.00, 9, 'Игровой набор Совтехстром Кегли и шары', 'F735B6.jpg'),
('F937G4', 'Игровой набор', 'шт.', 480.00, 10.00, 7, 1, 1, 4.00, 12, 'Набор Abtoys Бадминтон и теннис', 'F937G4.jpg'),
('E324U7', 'Велотренажер', 'шт.', 6480.00, 25.00, 8, 1, 1, 5.00, 5, 'Велотренажер двойной DFC B804 dual bike', 'E324U7.jpg'),
('G403T5', 'Тюбинг', 'шт.', 1450.00, 15.00, 9, 1, 1, 4.00, 13, 'Тюбинг Nordway, 73 см', 'G403T5.jpg'),
('N483G5', 'Клюшка', 'шт.', 1299.00, 10.00, 9, 2, 1, 3.00, 4, 'Клюшка Nordway NDW300 (2019/2020) SR лев. 19 150см', NULL),
('D038G6', 'Лыжный комплект', 'шт.', 3000.00, 30.00, 9, 2, 1, 4.00, 23, 'Лыжный комплект беговые NORDWAY XC Classic, 45-45-45мм, 160см', NULL),
('G480F5', 'Ролики', 'шт.', 1600.00, 15.00, 10, 1, 1, 4.00, 7, 'Коньки роликовые Ridex Cricket жен. ABEC 3 кол.:72мм р.:39-42 синий', NULL),
('C324S5', 'Шлем', 'шт.', 4000.00, 10.00, 11, 2, 1, 5.00, 16, 'Шлем г.л./сноуб. Salomon Grom р.:KS черный (L40836800)', NULL),
('V312R4', 'Мяч', 'шт.', 4150.00, 20.00, 12, 2, 1, 2.00, 5, 'Мяч волейбольный MIKASA VT370W, для зала, 5-й размер, желтый/синий', NULL),
('J4DF5E', 'Насос', 'шт.', 300.00, 5.00, 13, 1, 1, 4.00, 12, 'Насос Molten HP-18-B для мячей мультиколор', NULL),
('G522B5', 'Ласты', 'шт.', 1980.00, 15.00, 14, 2, 1, 3.00, 6, 'Ласты Colton CF-02 для плавания р.:33-34 серый/голубой', NULL),
('K432G6', 'Шапочка для плавания', 'шт.', 440.00, 25.00, 15, 2, 1, 5.00, 17, 'Шапочка для плавания Atemi PU 140 ткань с покрытием желтый', NULL),
('J532D4', 'Перчатки для карате', 'шт.', 1050.00, 15.00, 16, 1, 1, 3.00, 5, 'Перчатки для каратэ Green Hill KMС-6083 L красный', NULL),
('G873H4', 'Велосипед', 'шт.', 14930.00, 5.00, 17, 1, 1, 4.00, 6, 'Велосипед SKIF 29 Disc (2021), горный (взрослый), рама: 17", колеса: 29", темно-серый', NULL),
('V423D4', 'Штанга', 'шт.', 5600.00, 10.00, 18, 2, 1, 3.00, 8, 'Штанга Starfit BB-401 30кг пласт. черный', NULL),
('K937A5', 'Гиря', 'шт.', 890.00, 5.00, 18, 2, 1, 4.00, 10, 'Гиря Starfit ГМБ4 мягкое 4кг синий/оранжевый', NULL),
('F047J7', 'Коврик', 'шт.', 720.00, 15.00, 19, 1, 1, 5.00, 11, 'Коврик Bradex для мягкой йоги дл.:1730мм ш.:610мм т.:3мм серый', NULL),
('S374B5', 'Ролик для йоги', 'шт.', 700.00, 10.00, 19, 1, 1, 3.00, 12, 'Ролик для йоги Bradex Туба d=14см ш.:33см оранжевый', NULL),
('F687G5', 'Защита голени', 'шт.', 1900.00, 15.00, 16, 1, 1, 4.00, 6, 'Защита голени GREEN HILL Panther, L, синий/черный', NULL),
('N892G6', 'Очки для плавания', 'шт.', 500.00, 5.00, 15, 2, 1, 5.00, 14, 'Очки для плавания Atemi N8401 синий', NULL),
('D893W4', 'Мяч', 'шт.', 900.00, 5.00, 20, 1, 1, 2.00, 5, 'Мяч футбольный DEMIX 1STLS1JWWW, универсальный, 4-й размер, белый/зеленый', NULL),
('N836R5', 'Коньки', 'шт.', 2000.00, 10.00, 15, 2, 1, 3.00, 16, 'Коньки ATEMI AKSK01DXS, раздвижные, прогулочные, унисекс, 27-30, черный/зеленый', NULL),
('D927K3', 'Перчатки', 'шт.', 660.00, 15.00, 18, 2, 1, 4.00, 3, 'Перчатки Starfit SU-125 атлетические S черный', NULL),
('V392H7', 'Степ-платформа', 'шт.', 4790.00, 10.00, 18, 1, 1, 3.00, 15, 'Степ-платформа Starfit SP-204 серый/черный', NULL);
GO

-- 7. Вставляем заказы из Заказ_import.xlsx
-- Сначала создаем заказы
INSERT INTO Orders (OrderNumber, UserId, OrderDate, DeliveryDate, DeliveryPointId, PickupCode, Status) VALUES
('ORD001', 7, '2022-05-15', '2022-05-21', 18, '401', 'Новый'),
('ORD002', 8, '2022-05-16', '2022-05-22', 20, '402', 'Новый'),
('ORD003', 9, '2022-05-17', '2022-05-23', 20, '403', 'Завершен'),
('ORD004', 10, '2022-05-18', '2022-05-24', 22, '404', 'Новый'),
('ORD005', NULL, '2022-05-19', '2022-05-25', 22, '405', 'Новый'),
('ORD006', NULL, '2022-05-19', '2022-05-25', 16, '406', 'Новый'),
('ORD007', NULL, '2022-05-21', '2022-05-27', 16, '407', 'Завершен'),
('ORD008', NULL, '2022-05-22', '2022-05-28', 18, '408', 'Завершен'),
('ORD009', NULL, '2022-05-23', '2022-05-29', 24, '409', 'Новый'),
('ORD010', NULL, '2022-05-24', '2022-05-30', 24, '410', 'Завершен');
GO

-- 8. Вставляем элементы заказов
DECLARE @OrderId INT, @ProductId INT;

-- Заказ 1: А112Т4, 2, G598Y6, 2
SELECT @OrderId = Id FROM Orders WHERE OrderNumber = 'ORD001';
SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'А112Т4';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 2, (SELECT Price FROM Products WHERE Id = @ProductId));

SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'G598Y6';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 2, (SELECT Price FROM Products WHERE Id = @ProductId));

-- Заказ 2: F746E6, 3, D830R5, 3
SELECT @OrderId = Id FROM Orders WHERE OrderNumber = 'ORD002';
SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'F746E6';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 3, (SELECT Price FROM Products WHERE Id = @ProductId));

SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'D830R5';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 3, (SELECT Price FROM Products WHERE Id = @ProductId));

-- Заказ 3: D648N7, 10, F735B6, 10
SELECT @OrderId = Id FROM Orders WHERE OrderNumber = 'ORD003';
SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'D648N7';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 10, (SELECT Price FROM Products WHERE Id = @ProductId));

SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'F735B6';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 10, (SELECT Price FROM Products WHERE Id = @ProductId));

-- Заказ 4: F937G4, 1, E324U7, 1
SELECT @OrderId = Id FROM Orders WHERE OrderNumber = 'ORD004';
SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'F937G4';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 1, (SELECT Price FROM Products WHERE Id = @ProductId));

SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'E324U7';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 1, (SELECT Price FROM Products WHERE Id = @ProductId));

-- Заказ 5: N483G5, 10, D038G6, 10
SELECT @OrderId = Id FROM Orders WHERE OrderNumber = 'ORD005';
SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'N483G5';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 10, (SELECT Price FROM Products WHERE Id = @ProductId));

SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'D038G6';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 10, (SELECT Price FROM Products WHERE Id = @ProductId));

-- Заказ 6: G480F5, 2, C324S5, 2
SELECT @OrderId = Id FROM Orders WHERE OrderNumber = 'ORD006';
SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'G480F5';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 2, (SELECT Price FROM Products WHERE Id = @ProductId));

SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'C324S5';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 2, (SELECT Price FROM Products WHERE Id = @ProductId));

-- Заказ 7: V312R4, 1, J4DF5E, 1
SELECT @OrderId = Id FROM Orders WHERE OrderNumber = 'ORD007';
SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'V312R4';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 1, (SELECT Price FROM Products WHERE Id = @ProductId));

SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'J4DF5E';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 1, (SELECT Price FROM Products WHERE Id = @ProductId));

-- Заказ 8: G522B5, 3, K432G6, 3
SELECT @OrderId = Id FROM Orders WHERE OrderNumber = 'ORD008';
SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'G522B5';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 3, (SELECT Price FROM Products WHERE Id = @ProductId));

SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'K432G6';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 3, (SELECT Price FROM Products WHERE Id = @ProductId));

-- Заказ 9: F047J7, 1, S374B5, 1
SELECT @OrderId = Id FROM Orders WHERE OrderNumber = 'ORD009';
SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'F047J7';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 1, (SELECT Price FROM Products WHERE Id = @ProductId));

SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'S374B5';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 1, (SELECT Price FROM Products WHERE Id = @ProductId));

-- Заказ 10: N836R5, 5, D927K3, 5
SELECT @OrderId = Id FROM Orders WHERE OrderNumber = 'ORD010';
SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'N836R5';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 5, (SELECT Price FROM Products WHERE Id = @ProductId));

SELECT @ProductId = Id FROM Products WHERE ArticleNumber = 'D927K3';
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (@OrderId, @ProductId, 5, (SELECT Price FROM Products WHERE Id = @ProductId));
GO

-- 9. Обновляем общую сумму заказов
UPDATE Orders 
SET TotalAmount = (
    SELECT SUM(Quantity * UnitPrice) 
    FROM OrderItems 
    WHERE OrderId = Orders.Id
);
GO

-- Проверяем данные
SELECT 'Users' AS TableName, COUNT(*) AS RecordCount FROM Users
UNION ALL
SELECT 'DeliveryPoints', COUNT(*) FROM DeliveryPoints
UNION ALL
SELECT 'Categories', COUNT(*) FROM Categories
UNION ALL
SELECT 'Manufacturers', COUNT(*) FROM Manufacturers
UNION ALL
SELECT 'Suppliers', COUNT(*) FROM Suppliers
UNION ALL
SELECT 'Products', COUNT(*) FROM Products
UNION ALL
SELECT 'Orders', COUNT(*) FROM Orders
UNION ALL
SELECT 'OrderItems', COUNT(*) FROM OrderItems;
GO