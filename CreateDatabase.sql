-- сценарий создания БД и таблиц
IF DB_ID('MuseumTicketsDB') IS NOT NULL
BEGIN
    DROP DATABASE MuseumTicketsDB;
END
GO

CREATE DATABASE MuseumTicketsDB;
GO

USE MuseumTicketsDB;
GO

IF OBJECT_ID('MuseumExhibition', 'U') IS NOT NULL 
	DROP TABLE MuseumExhibition;
IF OBJECT_ID('OrderItem', 'U') IS NOT NULL 
	DROP TABLE OrderItem;
IF OBJECT_ID('[Order]', 'U') IS NOT NULL 
	DROP TABLE [Order];
IF OBJECT_ID('[User]', 'U') IS NOT NULL
	DROP TABLE [User];
IF OBJECT_ID('TicketPrice', 'U') IS NOT NULL 
	DROP TABLE TicketPrice;
IF OBJECT_ID('Ticket', 'U') IS NOT NULL 
	DROP TABLE Ticket;
IF OBJECT_ID('Exhibition', 'U') IS NOT NULL 
	DROP TABLE Exhibition;
IF OBJECT_ID('ScheduleExceptions', 'U') IS NOT NULL 
	DROP TABLE ScheduleExceptions;
IF OBJECT_ID('ScheduleDays', 'U') IS NOT NULL 
	DROP TABLE ScheduleDays;
IF OBJECT_ID('MuseumSchedule', 'U') IS NOT NULL 
	DROP TABLE MuseumSchedule;
IF OBJECT_ID('Museum', 'U') IS NOT NULL 
	DROP TABLE Museum;
IF OBJECT_ID('MuseumComplex', 'U') IS NOT NULL 
	DROP TABLE MuseumComplex;
GO


-- Музейный комплекс
CREATE TABLE MuseumComplex
(
    MuseumComplexID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL
);
GO

-- Музей
CREATE TABLE Museum
(
    MuseumID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    Street NVARCHAR(50) NOT NULL,
	House NVARCHAR(5) NOT NULL,
    MuseumComplexID INT NOT NULL,
    CONSTRAINT FK_Museum_MuseumComplex FOREIGN KEY (MuseumComplexID) 
		REFERENCES MuseumComplex(MuseumComplexID)
);
GO

-- График работы музея
CREATE TABLE MuseumSchedule 
(
    MuseumScheduleID INT IDENTITY(1,1) PRIMARY KEY,
    MuseumID INT NOT NULL UNIQUE,
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
    ScheduleDaysID INT IDENTITY(1,1) PRIMARY KEY,
    MuseumScheduleID INT NOT NULL,
    WeekDay INT NOT NULL CHECK (WeekDay BETWEEN 1 AND 7),
    CONSTRAINT FK_ScheduleDays_MuseumSchedule FOREIGN KEY (MuseumScheduleID) 
		REFERENCES MuseumSchedule(MuseumScheduleID)
);
GO

-- Исключения из графика работы
CREATE TABLE ScheduleExceptions 
(
    ScheduleExceptionsID INT IDENTITY(1,1) PRIMARY KEY,
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
    ExhibitionID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Photo NVARCHAR(255) NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL
);
GO

-- Ассоциативная таблица Музей-Экспозиция (для переносных выставок)
CREATE TABLE MuseumExhibition (
    MuseumExhibitionID INT IDENTITY(1,1) PRIMARY KEY,
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
    TicketID INT IDENTITY(1,1) PRIMARY KEY,
    ExhibitionID INT NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Ticket_Exhibition FOREIGN KEY (ExhibitionID) 
		REFERENCES Exhibition(ExhibitionID)
);
GO

-- Цена билета
CREATE TABLE TicketPrice 
(
    TicketPriceID INT IDENTITY(1,1) PRIMARY KEY,
    TicketID INT NOT NULL,
    Price MONEY NOT NULL,
    CONSTRAINT FK_TicketPrice_Ticket FOREIGN KEY (TicketID) 
		REFERENCES Ticket(TicketID)
);
GO

-- Пользователь
CREATE TABLE [User]
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,
	LastName NVARCHAR(50) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(50) NULL,
    Email NVARCHAR(50) NOT NULL UNIQUE,
    Phone NVARCHAR(20) NULL,
    Password NVARCHAR(100) NOT NULL,
    Role NVARCHAR(50) NOT NULL
);
GO

-- Заказ
CREATE TABLE [Order] 
(
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Order_User FOREIGN KEY (UserID) 
		REFERENCES [User](UserID)
);
GO

-- Позиции заказа
CREATE TABLE OrderItem 
(
    OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
    OrderID INT NOT NULL,
    TicketID INT NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_OrderItem_Order FOREIGN KEY (OrderID) 
		REFERENCES [Order](OrderID),
    CONSTRAINT FK_OrderItem_Ticket FOREIGN KEY (TicketID) 
		REFERENCES Ticket(TicketID)
);
GO