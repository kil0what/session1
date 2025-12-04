-- Создание базы данных
CREATE DATABASE SportsGoodsDB;
GO

USE SportsGoodsDB;
GO

-- Таблица пользователей
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Login NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Manager', 'Client', 'Guest')),
    Email NVARCHAR(100),
    IsActive BIT DEFAULT 1
);
GO

-- Таблица товаров
CREATE TABLE Products (
    ArticleNumber NVARCHAR(20) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Unit NVARCHAR(20) DEFAULT 'шт.',
    Price DECIMAL(10,2) NOT NULL,
    MaxDiscount DECIMAL(5,2) DEFAULT 0,
    Manufacturer NVARCHAR(100),
    Supplier NVARCHAR(100),
    Category NVARCHAR(50),
    CurrentDiscount DECIMAL(5,2) DEFAULT 0,
    StockQuantity INT DEFAULT 0,
    Description NVARCHAR(1000),
    ImagePath NVARCHAR(500),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    IsDeleted BIT DEFAULT 0
);
GO

-- Таблица заказов
CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES Users(Id),
    OrderDate DATETIME DEFAULT GETDATE(),
    DeliveryDate DATETIME,
    PickupPointId INT,
    CustomerName NVARCHAR(100),
    Code NVARCHAR(20),
    Status NVARCHAR(20) DEFAULT 'Новый',
    TotalAmount DECIMAL(10,2),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO

-- Таблица позиций заказа
CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ArticleNumber NVARCHAR(20) NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Discount DECIMAL(5,2) DEFAULT 0,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ArticleNumber) REFERENCES Products(ArticleNumber)
);
GO

-- Таблица пунктов выдачи
CREATE TABLE PickupPoints (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Address NVARCHAR(500) NOT NULL,
    Phone NVARCHAR(20),
    WorkingHours NVARCHAR(100)
);
GO