-- сценарий создания БД и таблиц
IF DB_ID('MuseumTicketsDB') IS NOT NULL
BEGIN
    ALTER DATABASE MuseumTicketsDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MuseumTicketsDB;
END
GO

CREATE DATABASE MuseumTicketsDB;
GO

USE MuseumTicketsDB;
GO

-- Музейный комплекс
CREATE TABLE MuseumComplex
(
    MuseumComplexID INT IDENTITY(10001,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL
);
GO

-- Музей
CREATE TABLE Museum
(
    MuseumID INT IDENTITY(10001,1) PRIMARY KEY,
    MuseumComplexID INT NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    Street NVARCHAR(50) NOT NULL,
    House NVARCHAR(5) NOT NULL,
    CONSTRAINT FK_Museum_MuseumComplex FOREIGN KEY (MuseumComplexID) 
        REFERENCES MuseumComplex(MuseumComplexID)
);
GO

-- График работы музея
CREATE TABLE MuseumSchedule 
(
    MuseumScheduleID INT IDENTITY(10001,1) PRIMARY KEY,
    MuseumID INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    OpenTime TIME NOT NULL,
    CloseTime TIME NOT NULL,
    CONSTRAINT FK_MuseumSchedule_Museum FOREIGN KEY (MuseumID) 
        REFERENCES Museum(MuseumID)
);
GO

-- Дни недели для графика
CREATE TABLE ScheduleDays 
(
    ScheduleDaysID INT IDENTITY(10001,1) PRIMARY KEY,
    MuseumScheduleID INT NOT NULL,
    WeekDay INT NOT NULL CHECK (WeekDay BETWEEN 1 AND 7),
    CONSTRAINT FK_ScheduleDays_MuseumSchedule FOREIGN KEY (MuseumScheduleID) 
        REFERENCES MuseumSchedule(MuseumScheduleID)
);
GO

-- Исключения из графика работы
CREATE TABLE ScheduleExceptions 
(
    ScheduleExceptionsID INT IDENTITY(10001,1) PRIMARY KEY,
    MuseumScheduleID INT NOT NULL,
    ExceptionDate DATE NOT NULL,
    IsOpen BIT NOT NULL,
    OpenTime TIME NULL,
    CloseTime TIME NULL,
    CONSTRAINT FK_ScheduleExceptions_MuseumSchedule FOREIGN KEY (MuseumScheduleID) 
        REFERENCES MuseumSchedule(MuseumScheduleID)
);
GO

-- Экспозиция
CREATE TABLE Exhibition 
(
    ExhibitionID INT IDENTITY(10001,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Photo NVARCHAR(255) NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL
);
GO

-- Ассоциативная таблица Музей-Экспозиция
CREATE TABLE MuseumExhibition (
    MuseumExhibitionID INT IDENTITY(10001,1) PRIMARY KEY,
    MuseumID INT NOT NULL,
    ExhibitionID INT NOT NULL,
    CONSTRAINT FK_MuseumExhibition_Museum FOREIGN KEY (MuseumID) 
        REFERENCES Museum(MuseumID),
    CONSTRAINT FK_MuseumExhibition_Exhibition FOREIGN KEY (ExhibitionID) 
        REFERENCES Exhibition(ExhibitionID)
);
GO

-- Билет
CREATE TABLE Ticket 
(
    TicketID INT IDENTITY(10001,1) PRIMARY KEY,
    ExhibitionID INT NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    -- Добавили N перед строками в CHECK
    Status NVARCHAR(20) NOT NULL CHECK (Status IN (N'Доступен', N'Продан', N'Отменён')),
    AvailableQuantity INT NOT NULL CHECK (AvailableQuantity >= 0),
    CONSTRAINT FK_Ticket_Exhibition FOREIGN KEY (ExhibitionID)
        REFERENCES Exhibition(ExhibitionID),
    CONSTRAINT UQ_Ticket_Exhibition_Type UNIQUE (ExhibitionID, Type)
);
GO

-- Цена билета
CREATE TABLE TicketPrice 
(
    TicketPriceID INT IDENTITY(10001,1) PRIMARY KEY,
    TicketID INT NOT NULL,
    Price MONEY NOT NULL CHECK (Price >= 0),
    CONSTRAINT FK_TicketPrice_Ticket FOREIGN KEY (TicketID) 
        REFERENCES Ticket(TicketID)
);
GO

-- Пользователь
CREATE TABLE [User]
(
    UserID INT IDENTITY(10001,1) PRIMARY KEY,
    LastName NVARCHAR(50) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50) NULL,
    Email NVARCHAR(50) NOT NULL UNIQUE,
    Phone NVARCHAR(20) NULL,
    Password NVARCHAR(100) NOT NULL,
    -- Добавили N в CHECK
    Role NVARCHAR(50) NOT NULL
        CHECK (Role IN (N'Гость', N'Посетитель', N'Администратор музея', N'Администратор системы'))
);
GO

-- Заказ
CREATE TABLE [Order] 
(
    OrderID INT IDENTITY(10001,1) PRIMARY KEY,
    UserID INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    -- Добавили N в CHECK
    Status NVARCHAR(50) NOT NULL CHECK (Status IN (N'В ожидании', N'Оплачен', N'Отменён')),
    CONSTRAINT FK_Order_User FOREIGN KEY (UserID) 
        REFERENCES [User](UserID)
);
GO

-- Позиции заказа
CREATE TABLE OrderItem 
(
    OrderItemID INT IDENTITY(10001,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    TicketID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    CONSTRAINT FK_OrderItem_Order FOREIGN KEY (OrderID) 
        REFERENCES [Order](OrderID),
    CONSTRAINT FK_OrderItem_Ticket FOREIGN KEY (TicketID) 
        REFERENCES Ticket(TicketID)
);
GO

-- Оплата
CREATE TABLE Payment
(
    PaymentID INT IDENTITY(10001,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    Amount MONEY NOT NULL CHECK (Amount >= 0),
    PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
    Success BIT NOT NULL,
    CONSTRAINT FK_Payment_Order FOREIGN KEY (OrderID)
        REFERENCES [Order](OrderID)
);
GO