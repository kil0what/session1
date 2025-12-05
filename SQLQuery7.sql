-- *********************************************************************************
-- Секция 1: Удаление и Создание Базы Данных с кириллической коллацией
-- *********************************************************************************

-- Переключаемся на системную базу, чтобы не блокировать целевую базу
USE master;
GO

-- Надежное удаление базы данных, если она существует
IF DB_ID(N'SportsGoodsDB_Cyrillic') IS NOT NULL
BEGIN
    -- Принудительное закрытие всех активных соединений
    EXEC('ALTER DATABASE SportsGoodsDB_Cyrillic SET SINGLE_USER WITH ROLLBACK IMMEDIATE;');
    -- Удаление базы данных
    EXEC('DROP DATABASE SportsGoodsDB_Cyrillic;');
END
GO

-- Создание новой базы данных с обязательной коллацией для кириллицы
CREATE DATABASE SportsGoodsDB_Cyrillic
COLLATE Cyrillic_General_CI_AS;
GO

USE SportsGoodsDB_Cyrillic;
GO

-- *********************************************************************************
-- Секция 2: Создание Таблиц (Структура по ТЗ)
-- *********************************************************************************

-- 1. Таблица PickupPoints (Пункты выдачи)
CREATE TABLE PickupPoints (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Address NVARCHAR(1000) NOT NULL,
    Phone NVARCHAR(40) NULL,
    WorkingHours NVARCHAR(200) NULL
);

-- 2. Таблица Users (Пользователи)
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Login NVARCHAR(200) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(400) NOT NULL,
    Role NVARCHAR(100) NOT NULL, -- (Администратор/Менеджер/Клиент)
    Email NVARCHAR(200) NULL
);

-- 3. Таблица Products (Товары)
CREATE TABLE Products (
    ArticleNumber NVARCHAR(40) PRIMARY KEY,
    Name NVARCHAR(400) NOT NULL,
    Unit NVARCHAR(40) NULL,
    Price DECIMAL(18,2) NOT NULL,
    MaxDiscount DECIMAL(5,2) NULL,
    Manufacturer NVARCHAR(200) NULL,
    Supplier NVARCHAR(200) NULL,
    Category NVARCHAR(200) NULL,
    CurrentDiscount DECIMAL(5,2) NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    Description NVARCHAR(MAX) NULL,
    ImagePath NVARCHAR(1000) NULL,
    CONSTRAINT CHK_Discount CHECK (MaxDiscount IS NULL OR (MaxDiscount >= 0 AND MaxDiscount <= 100)),
    CONSTRAINT CHK_CurrentDiscount CHECK (CurrentDiscount IS NULL OR (CurrentDiscount >= 0 AND CurrentDiscount <= 100))
);

-- 4. Таблица Orders (Заказы)
CREATE TABLE Orders (
    Id INT PRIMARY KEY,
    OrderItems NVARCHAR(MAX) NULL,
    OrderDate DATETIME NOT NULL,
    DeliveryDate DATETIME NULL,
    PickupPointId INT NULL,
    CustomerName NVARCHAR(400) NULL,
    Code NVARCHAR(50) NULL,
    Status NVARCHAR(50) NULL,
    CONSTRAINT FK_Orders_PickupPoints FOREIGN KEY (PickupPointId)
    REFERENCES PickupPoints(Id)
);
GO

-- *********************************************************************************
-- Секция 3: Заполнение Тестовыми Данными (ВСЕ ПРОВЕРЕНО)
-- *********************************************************************************

-- 1. PickupPoints (Пункты выдачи)
SET IDENTITY_INSERT PickupPoints ON;

INSERT INTO PickupPoints (Id, Address, Phone, WorkingHours) VALUES
(1, N'344288, г. Дубна, ул. Чехова, 1', NULL, NULL), (2, N'614164, г.Дубна, ул. Степная, 30', NULL, NULL),
(3, N'394242, г. Дубна, ул. Коммунистическая, 43', NULL, NULL), (4, N'660540, г. Дубна, ул. Солнечная, 25', NULL, NULL),
(5, N'125837, г. Дубна, ул. Шоссейная, 40', NULL, NULL), (6, N'125703, г. Дубна, ул. Партизанская, 49', NULL, NULL),
(7, N'625283, г. Дубна, ул. Победы, 46', NULL, NULL), (8, N'614611, г. Дубна, ул. Молодежная, 50', NULL, NULL),
(9, N'454311, г.Дубна, ул. Новая, 19', NULL, NULL), (10, N'660007, г.Дубна, ул. Октябрьская, 19', NULL, NULL),
(11, N'603036, г. Дубна, ул. Садовая, 4', NULL, NULL), (12, N'450983, г.Дубна, ул. Комсомольская, 26', NULL, NULL),
(13, N'394782, г. Дубна, ул. Чехова, 3', NULL, NULL), (14, N'603002, г. Дубна, ул. Дзержинского, 28', NULL, NULL),
(15, N'450558, г. Дубна, ул. Набережная, 30', NULL, NULL), (16, N'394060, г.Дубна, ул. Фрунзе, 43', NULL, NULL), 
(17, N'410661, г. Дубна, ул. Школьная, 50', NULL, NULL), (18, N'625590, г. Дубна, ул. Коммунистическая, 20', NULL, NULL), 
(19, N'625683, г. Дубна, ул. 8 Марта', NULL, NULL), (20, N'400562, г. Дубна, ул. Зеленая, 32', NULL, NULL), 
(21, N'614510, г. Дубна, ул. Маяковского, 47', NULL, NULL), (22, N'410542, г. Дубна, ул. Светлая, 46', NULL, NULL), 
(23, N'620839, г. Дубна, ул. Цветочная, 8', NULL, NULL), (24, N'443890, г. Дубна, ул. Коммунистическая, 1', NULL, NULL), 
(25, N'603379, г. Дубна, ул. Спортивная, 46', NULL, NULL), (26, N'603721, г. Дубна, ул. Гоголя, 41', NULL, NULL),
(27, N'410172, г. Дубна, ул. Северная, 13', NULL, NULL), (28, N'420151, г. Дубна, ул. Вишневая, 32', NULL, NULL),
(29, N'125061, г. Дубна, ул. Подгорная, 8', NULL, NULL), (30, N'630370, г. Дубна, ул. Шоссейная, 24', NULL, NULL),
(31, N'614753, г. Дубна, ул. Полевая, 35', NULL, NULL), (32, N'426030, г. Дубна, ул. Маяковского, 44', NULL, NULL),
(33, N'450375, г. Дубна ул. Клубная, 44', NULL, NULL), (34, N'625560, г. Дубна, ул. Некрасова, 12', NULL, NULL),
(35, N'630201, г. Дубна, ул. Комсомольская, 17', NULL, NULL), (36, N'190949, г. Дубна, ул. Мичурина, 26', NULL, NULL);

SET IDENTITY_INSERT PickupPoints OFF;
DBCC CHECKIDENT ('PickupPoints', RESEED, 36);


-- 2. Users (Пользователи)
SET IDENTITY_INSERT Users ON;

INSERT INTO Users (Id, Role, FullName, Login, Password, Email) VALUES
(1, N'Администратор', N'Пахомова Аиша Анатольевна', N'm4ic8j5qgstw@gmail.com', N'2L6KZG', N'm4ic8j5qgstw@gmail.com'),
(2, N'Администратор', N'Жуков Роман Богданович', N'd43zfg9tlsyv@gmail.com', N'uzWC67', N'd43zfg9tlsyv@gmail.com'),
(3, N'Администратор', N'Киселева Анастасия Максимовна', N'8ohgisf6k45w@outlook.com', N'8ntwUp', N'8ohgisf6k45w@outlook.com'),
(4, N'Менеджер', N'Григорьева Арина Арсентьевна', N'hi1brwj46czx@mail.com', N'YOYhfR', N'hi1brwj46czx@mail.com'),
(5, N'Менеджер', N'Иванов Лев Михайлович', N'fvkbcamhlj52@gmail.com', N'RSbvHv', N'fvkbcamhlj52@gmail.com'),
(6, N'Менеджер', N'Григорьев Лев Давидович', N'9qxnce8jwruv@gmail.com', N'rwVDh9', N'9qxnce8jwruv@gmail.com'),
(7, N'Клиент', N'Поляков Степан Егорович', N'dotiex942p1r@gmail.com', N'LdNyos', N'dotiex942p1r@gmail.com'),
(8, N'Клиент', N'Леонова Алиса Кирилловна', N'n0bmi2h1xral@tutanota.com', N'gynQMT', N'n0bmi2h1xral@tutanota.com'),
(9, N'Клиент', N'Яковлев Платон Константинович', N'sfm3t278kdvz@yahoo.com', N'AtnDjr', N'sfm3t278kdvz@yahoo.com'),
(10, N'Клиент', N'Ковалева Ева Яковлевна', N'ilb8rdut0v7e@mail.com', N'JlFRCZ', N'ilb8rdut0v7e@mail.com');

SET IDENTITY_INSERT Users OFF;
DBCC CHECKIDENT ('Users', RESEED, 10);

-- 3. Products (Товары)
INSERT INTO Products (ArticleNumber, Name, Unit, Price, MaxDiscount, Manufacturer, Supplier, Category, CurrentDiscount, StockQuantity, Description, ImagePath) VALUES
(N'А112Т4', N'Боксерская груша', N'шт.', 778.00, 30.00, N'X-Match', N'Спортмастер', N'Спортивный инвентарь', 5.00, 6, N'Боксерская груша X-Match черная', N'А112Т4.jpg'),
(N'G598Y6', N'Спортивный мат', N'шт.', 2390.00, 15.00, N'Perfetto Sport', N'Декатлон', N'Спортивный инвентарь', 2.00, 16, N'Спортивный мат 100x100x10 см Perfetto Sport № 3 бежевый', N'G598Y6.jpg'),
(N'F746E6', N'Шведская стенка', N'шт.', 9900.00, 10.00, N'ROMANA Next', N'Декатлон', N'Спортивный инвентарь', 3.00, 5, N'Шведская стенка ROMANA Next, pastel', N'F746E6.jpg'),
(N'D830R5', N'Тренажер прыжков', N'шт.', 1120.00, 15.00, N'Moby Kids', N'Спортмастер', N'Спортивный инвентарь', 4.00, 8, N'Тренажер для прыжков Moby Kids Moby-Jumper со счетчиком', N'D830R5.jpg'),
(N'B538G6', N'Спортивный костюм', N'шт.', 839.00, 5.00, N'playToday', N'Спортмастер', N'Одежда', 3.00, 17, N'Спортивный костюм playToday (футболка + шорты)', N'B538G6.jpg'),
(N'D648N7', N'Набор для хоккея', N'шт.', 350.00, 10.00, N'Совтехстром', N'Декатлон', N'Спортивный инвентарь', 4.00, 7, N'Набор для хоккея Совтехстром', N'D648N7.jpg'),
(N'F735B6', N'Игровой набор', N'шт.', 320.00, 15.00, N'Совтехстром', N'Декатлон', N'Спортивный инвентарь', 2.00, 9, N'Игровой набор Совтехстром Кегли и шары', N'F735B6.jpg'),
(N'F937G4', N'Игровой набор', N'шт.', 480.00, 10.00, N'Abtoys', N'Спортмастер', N'Спортивный инвентарь', 4.00, 12, N'Набор Abtoys Бадминтон и теннис', N'F937G4.jpg'),
(N'E324U7', N'Велотренажер', N'шт.', 6480.00, 25.00, N'DFC', N'Спортмастер', N'Спортивный инвентарь', 5.00, 5, N'Велотренажер двойной DFC B804 dual bike', N'E324U7.jpg'),
(N'G403T5', N'Тюбинг', N'шт.', 1450.00, 15.00, N'Nordway', N'Спортмастер', N'Спортивный инвентарь', 4.00, 13, N'Тюбинг Nordway, 73 см', N'G403T5.jpg'),
(N'N483G5', N'Клюшка', N'шт.', 1299.00, 10.00, N'Nordway', N'Декатлон', N'Спортивный инвентарь', 3.00, 4, N'Клюшка Nordway NDW300 (2019/2020) SR лев. 19 150см', N'N483G5.jpg'),
(N'D038G6', N'Лыжный комплект', N'шт.', 3000.00, 30.00, N'Nordway', N'Декатлон', N'Спортивный инвентарь', 4.00, 23, N'Лыжный комплект беговые NORDWAY XC Classic, 45-45-45мм, 160см', N'D038G6.jpg'),
(N'G480F5', N'Ролики', N'шт.', 1600.00, 15.00, N'Ridex', N'Спортмастер', N'Спортивный инвентарь', 4.00, 7, N'Коньки роликовые Ridex Cricket жен. ABEC 3 кол.:72мм р.:39-42 синий', N'G480F5.jpg'),
(N'C324S5', N'Шлем', N'шт.', 4000.00, 10.00, N'Salomon', N'Декатлон', N'Спортивный инвентарь', 5.00, 16, N'Шлем г.л./сноуб. Salomon Grom р.:KS черный (L40836800)', N'C324S5.jpg'),
(N'V312R4', N'Мяч', N'шт.', 4150.00, 20.00, N'Mikasa', N'Декатлон', N'Спортивный инвентарь', 2.00, 5, N'Мяч волейбольный MIKASA VT370W, для зала, 5-й размер, желтый/синий', N'V312R4.jpg'),
(N'J4DF5E', N'Насос', N'шт.', 300.00, 5.00, N'Molten', N'Спортмастер', N'Спортивный инвентарь', 4.00, 12, N'Насос Molten HP-18-B для мячей мультиколор', N'J4DF5E.jpg'),
(N'G522B5', N'Ласты', N'шт.', 1980.00, 15.00, N'Colton', N'Декатлон', N'Спортивный инвентарь', 3.00, 6, N'Ласты Colton CF-02 для плавания р.:33-34 серый/голубой', N'G522B5.jpg'),
(N'K432G6', N'Шапочка для плавания', N'шт.', 440.00, 25.00, N'Atemi', N'Декатлон', N'Спортивный инвентарь', 5.00, 17, N'Шапочка для плавания Atemi PU 140 ткань с покрытием желтый', N'K432G6.jpg'),
(N'J532D4', N'Перчатки для карате', N'шт.', 1050.00, 15.00, N'Green Hill', N'Спортмастер', N'Спортивный инвентарь', 3.00, 5, N'Перчатки для каратэ Green Hill KMС-6083 L красный', N'J532D4.jpg'),
(N'G873H4', N'Велосипед', N'шт.', 14930.00, 5.00, N'SKIF', N'Спортмастер', N'Спортивный инвентарь', 4.00, 6, N'Велосипед SKIF 29 Disc (2021), горный (взрослый), рама: 17", колеса: 29", темно-серый', N'G873H4.jpg'),
(N'V423D4', N'Штанга', N'шт.', 5600.00, 10.00, N'Starfit', N'Декатлон', N'Спортивный инвентарь', 3.00, 8, N'Штанга Starfit BB-401 30кг пласт. черный', N'V423D4.jpg'),
(N'K937A5', N'Гиря', N'шт.', 890.00, 5.00, N'Starfit', N'Декатлон', N'Спортивный инвентарь', 4.00, 10, N'Гиря Starfit ГМБ4 мягкое 4кг синий/оранжевый', N'K937A5.jpg'),
(N'F047J7', N'Коврик', N'шт.', 720.00, 15.00, N'Bradex', N'Спортмастер', N'Спортивный инвентарь', 5.00, 11, N'Коврик Bradex для мягкой йоги дл.:1730мм ш.:610мм т.:3мм серый', N'F047J7.jpg'),
(N'S374B5', N'Ролик для йоги', N'шт.', 700.00, 10.00, N'Bradex', N'Спортмастер', N'Спортивный инвентарь', 3.00, 12, N'Ролик для йоги Bradex Туба d=14см ш.:33см оранжевый', N'S374B5.jpg'),
(N'N892G6', N'Очки для плавания', N'шт.', 500.00, 5.00, N'Atemi', N'Декатлон', N'Спортивный инвентарь', 5.00, 14, N'Очки для плавания Atemi N8401 синий', N'N892G6.jpg'),
(N'D893W4', N'Мяч', N'шт.', 900.00, 5.00, N'Demix', N'Спортмастер', N'Спортивный инвентарь', 2.00, 5, N'Мяч футбольный DEMIX 1STLS1JWWW, универсальный, 4-й размер, белый/зеленый', N'D893W4.jpg'),
(N'N836R5', N'Коньки', N'шт.', 2000.00, 10.00, N'Atemi', N'Декатлон', N'Спортивный инвентарь', 3.00, 16, N'Коньки ATEMI AKSK01DXS, раздвижные, прогулочные, унисекс, 27-30, черный/зеленый', N'N836R5.jpg'),
(N'D927K3', N'Перчатки', N'шт.', 660.00, 15.00, N'Starfit', N'Декатлон', N'Спортивный инвентарь', 4.00, 3, N'Перчатки Starfit SU-125 атлетические S черный', N'D927K3.jpg'),
(N'V392H7', N'Степ-платформа', N'шт.', 4790.00, 10.00, N'Starfit', N'Спортмастер', N'Спортивный инвентарь', 3.00, 15, N'Степ-платформа Starfit SP-204 серый/черный', N'V392H7.jpg');

-- *********************************************************************************
-- Секция 4: Orders (Заказы) - ИСПРАВЛЕНО: Безопасный формат даты
-- *********************************************************************************

-- 4. Orders (Заказы) - Используем безопасный формат даты 'YYYYMMDD HH:MM:SS'
INSERT INTO Orders (Id, OrderItems, OrderDate, DeliveryDate, PickupPointId, CustomerName, Code, Status) VALUES
(1, N'А112Т4, 2, G598Y6, 2', '20220515 00:00:00', '20220521 00:00:00', 18, N'Поляков Степан Егорович', N'401', N'Новый'),
(2, N'F746E6, 3, D830R5, 3', '20220516 00:00:00', '20220522 00:00:00', 20, N'Леонова Алиса Кирилловна', N'402', N'Новый'),
(3, N'D648N7, 10, F735B6, 10', '20220517 00:00:00', '20220523 00:00:00', 20, N'Яковлев Платон Константинович', N'403', N'Завершен'),
(4, N'F937G4, 1, E324U7, 1', '20220518 00:00:00', '20220524 00:00:00', 22, N'Ковалева Ева Яковлевна', N'404', N'Новый'),
(5, N'N483G5, 10, D038G6, 10', '20220519 00:00:00', '20220525 00:00:00', 22, NULL, N'405', N'Новый'),
(6, N'G480F5, 2, C324S5, 2', '20220519 00:00:00', '20220525 00:00:00', 16, NULL, N'406', N'Новый'),
(7, N'V312R4, 1, J4DF5E, 1', '20220521 00:00:00', '20220527 00:00:00', 16, NULL, N'407', N'Завершен'),
(8, N'G522B5, 3, K432G6, 3', '20220522 00:00:00', '20220528 00:00:00', 18, NULL, N'408', N'Завершен'),
(9, N'F047J7, 1, S374B5, 1', '20220523 00:00:00', '20220529 00:00:00', 24, NULL, N'409', N'Новый'),
(10, N'N836R5, 5, D927K3, 5', '20220524 00:00:00', '20220530 00:00:00', 24, NULL, N'410', N'Завершен');
GO

-- *********************************************************************************
-- Секция 5: Создание Индексов для производительности
-- *********************************************************************************

CREATE NONCLUSTERED INDEX IX_Users_Login ON Users (Login);
CREATE NONCLUSTERED INDEX IX_Users_FullName ON Users (FullName);
CREATE NONCLUSTERED INDEX IX_Products_Name ON Products (Name);
CREATE NONCLUSTERED INDEX IX_Products_Category ON Products (Category);
CREATE NONCLUSTERED INDEX IX_Orders_CustomerName ON Orders (CustomerName);
CREATE NONCLUSTERED INDEX IX_Orders_PickupPointId ON Orders (PickupPointId);
GO

-- *********************************************************************************
-- Секция 6: Хранимые Процедуры
-- *********************************************************************************

CREATE PROCEDURE dbo.AuthenticateUser
    @Login NVARCHAR(200),
    @Password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        Id,
        Login,
        FullName,
        Role,
        Email
    FROM
        Users
    WHERE
        Login = @Login
        AND Password = @Password;
END
GO

CREATE PROCEDURE dbo.GetAllProducts
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        ArticleNumber,
        Name,
        Unit,
        Price,
        MaxDiscount,
        Manufacturer,
        Supplier,
        Category,
        CurrentDiscount,
        StockQuantity,
        Description,
        ImagePath
    FROM
        Products
    ORDER BY
        Name;
END
GO

-- *********************************************************************************
-- Секция 7: Проверочные Запросы
-- *********************************************************************************

-- 1. Проверка коллации базы данных
SELECT name AS DatabaseName, collation_name
FROM sys.databases
WHERE name = N'SportsGoodsDB_Cyrillic';

-- 2. Проверка русских символов
SELECT N'Тест кириллицы: привет мир! Пользователи, Товары, Заказы' AS RussianTest;

-- 3. Подсчет записей в таблицах
SELECT 'Users' AS TableName, COUNT(*) AS RecordCount FROM Users
UNION ALL SELECT 'Products', COUNT(*) FROM Products
UNION ALL SELECT 'Orders', COUNT(*) FROM Orders
UNION ALL SELECT 'PickupPoints', COUNT(*) FROM PickupPoints;

-- 4. Показать примеры данных
SELECT TOP 3 * FROM Users;
SELECT TOP 3 ArticleNumber, Name, Price, StockQuantity FROM Products;
SELECT TOP 3 Id, CustomerName, Status FROM Orders;
GO