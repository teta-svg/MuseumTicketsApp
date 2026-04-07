using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Museum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exhibition",
                columns: table => new
                {
                    ExhibitionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exhibition", x => x.ExhibitionID);
                });

            migrationBuilder.CreateTable(
                name: "MuseumComplex",
                columns: table => new
                {
                    MuseumComplexID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuseumComplex", x => x.MuseumComplexID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                    table.CheckConstraint("CHK_User_Role", "[Role] IN (N'Гость', N'Посетитель', N'Администратор музея', N'Администратор системы')");
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    TicketID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    ExhibitionID = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.TicketID);
                    table.CheckConstraint("CHK_Ticket_Quantity", "[AvailableQuantity] >= 0");
                    table.CheckConstraint("CHK_Ticket_Status", "[Status] IN (N'Доступен', N'Продан', N'Отменён')");
                    table.ForeignKey(
                        name: "FK_Ticket_Exhibition_ExhibitionID",
                        column: x => x.ExhibitionID,
                        principalTable: "Exhibition",
                        principalColumn: "ExhibitionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Museum",
                columns: table => new
                {
                    MuseumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    MuseumComplexID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    House = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Museum", x => x.MuseumID);
                    table.ForeignKey(
                        name: "FK_Museum_MuseumComplex_MuseumComplexID",
                        column: x => x.MuseumComplexID,
                        principalTable: "MuseumComplex",
                        principalColumn: "MuseumComplexID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderID);
                    table.CheckConstraint("CHK_Order_Status", "[Status] IN (N'В ожидании', N'Оплачен', N'Отменён')");
                    table.ForeignKey(
                        name: "FK_Order_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketPrice",
                columns: table => new
                {
                    TicketPriceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPrice", x => x.TicketPriceID);
                    table.CheckConstraint("CHK_TicketPrice_Price", "[Price] >= 0");
                    table.ForeignKey(
                        name: "FK_TicketPrice_Ticket_TicketID",
                        column: x => x.TicketID,
                        principalTable: "Ticket",
                        principalColumn: "TicketID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MuseumExhibition",
                columns: table => new
                {
                    MuseumExhibitionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    MuseumID = table.Column<int>(type: "int", nullable: false),
                    ExhibitionID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuseumExhibition", x => x.MuseumExhibitionID);
                    table.ForeignKey(
                        name: "FK_MuseumExhibition_Exhibition_ExhibitionID",
                        column: x => x.ExhibitionID,
                        principalTable: "Exhibition",
                        principalColumn: "ExhibitionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MuseumExhibition_Museum_MuseumID",
                        column: x => x.MuseumID,
                        principalTable: "Museum",
                        principalColumn: "MuseumID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MuseumSchedule",
                columns: table => new
                {
                    MuseumScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    MuseumID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    OpenTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    CloseTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuseumSchedule", x => x.MuseumScheduleID);
                    table.ForeignKey(
                        name: "FK_MuseumSchedule_Museum_MuseumID",
                        column: x => x.MuseumID,
                        principalTable: "Museum",
                        principalColumn: "MuseumID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    OrderItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PriceAtPurchase = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.OrderItemID);
                    table.CheckConstraint("CHK_OrderItem_Quantity", "[Quantity] > 0");
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_Ticket_TicketID",
                        column: x => x.TicketID,
                        principalTable: "Ticket",
                        principalColumn: "TicketID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentID);
                    table.CheckConstraint("CHK_Payment_Amount", "[Amount] >= 0");
                    table.ForeignKey(
                        name: "FK_Payment_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDays",
                columns: table => new
                {
                    ScheduleDaysID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    MuseumScheduleID = table.Column<int>(type: "int", nullable: false),
                    WeekDay = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDays", x => x.ScheduleDaysID);
                    table.CheckConstraint("CHK_ScheduleDays_WeekDay", "[WeekDay] BETWEEN 1 AND 7");
                    table.ForeignKey(
                        name: "FK_ScheduleDays_MuseumSchedule_MuseumScheduleID",
                        column: x => x.MuseumScheduleID,
                        principalTable: "MuseumSchedule",
                        principalColumn: "MuseumScheduleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleExceptions",
                columns: table => new
                {
                    ScheduleExceptionsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10001, 1"),
                    MuseumScheduleID = table.Column<int>(type: "int", nullable: false),
                    ExceptionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    OpenTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    CloseTime = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleExceptions", x => x.ScheduleExceptionsID);
                    table.ForeignKey(
                        name: "FK_ScheduleExceptions_MuseumSchedule_MuseumScheduleID",
                        column: x => x.MuseumScheduleID,
                        principalTable: "MuseumSchedule",
                        principalColumn: "MuseumScheduleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserID", "Email", "FirstName", "LastName", "MiddleName", "Password", "Phone", "Role" },
                values: new object[] { 10001, "admin@museum.ru", "Петр", "Админов", "Петрович", "$2a$11$0NR8Ir7eg9MNV9VFQsudROSeSoRodME7UsMzNNfmLla/e/gLzQyuK", "+70000000000", "Администратор системы" });

            migrationBuilder.CreateIndex(
                name: "IX_Museum_MuseumComplexID",
                table: "Museum",
                column: "MuseumComplexID");

            migrationBuilder.CreateIndex(
                name: "IX_MuseumExhibition_ExhibitionID",
                table: "MuseumExhibition",
                column: "ExhibitionID");

            migrationBuilder.CreateIndex(
                name: "IX_MuseumExhibition_MuseumID",
                table: "MuseumExhibition",
                column: "MuseumID");

            migrationBuilder.CreateIndex(
                name: "IX_MuseumSchedule_MuseumID",
                table: "MuseumSchedule",
                column: "MuseumID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserID",
                table: "Order",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderID",
                table: "OrderItem",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_TicketID",
                table: "OrderItem",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderID",
                table: "Payment",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDays_MuseumScheduleID",
                table: "ScheduleDays",
                column: "MuseumScheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleExceptions_MuseumScheduleID",
                table: "ScheduleExceptions",
                column: "MuseumScheduleID");

            migrationBuilder.CreateIndex(
                name: "UQ_Ticket_Exhibition_Type",
                table: "Ticket",
                columns: new[] { "ExhibitionID", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketPrice_TicketID",
                table: "TicketPrice",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "UQ__User__A9D10534D2118DAE",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.Sql(@"
USE MuseumTicketsDB;
GO

INSERT INTO MuseumComplex (Name) VALUES 
(N'Государственный Эрмитаж'),
(N'Государственный Русский музей');
GO

INSERT INTO Museum (MuseumComplexID, Name, City, Street, House) VALUES 
(10001, N'Зимний дворец', N'Санкт-Петербург', N'Дворцовая наб.', N'34'),
(10001, N'Главный штаб', N'Санкт-Петербург', N'Дворцовая пл.', N'6'),
(10001, N'Зимний дворец Петра I', N'Санкт-Петербург', N'Дворцовая наб.', N'32'),
(10001, N'Дворец Меншикова', N'Санкт-Петербург', N'Университетская наб.', N'15'),
(10001, N'Музей фарфора', N'Санкт-Петербург', N'пр. Обуховской Обороны', N'151'),
(10002, N'Михайловский дворец', N'Санкт-Петербург', N'Инженерная ул.', N'4'),
(10002, N'Корпус Бенуа', N'Санкт-Петербург', N'наб. канала Грибоедова', N'2'),
(10002, N'Мраморный дворец', N'Санкт-Петербург', N'Миллионная ул.', N'5'),
(10002, N'Строгановский дворец', N'Санкт-Петербург', N'Невский пр.', N'17'),
(10002, N'Михайловский замок', N'Санкт-Петербург', N'Садовая ул.', N'2');
GO


INSERT INTO Exhibition (Name, Photo) VALUES 
(N'Шедевры Рембрандта', NULL),
(N'Золото скифов', NULL),
(N'Импрессионисты', NULL),
(N'Искусство Востока', NULL),
(N'Петровская эпоха', NULL),
(N'История гвардии', NULL),
(N'Архитектура барокко', NULL),
(N'Быт аристократии', NULL),
(N'Фарфоровая сказка', NULL),
(N'Техника росписи', NULL),
(N'Русская икона', NULL),
(N'Передвижники', NULL),
(N'Авангард XX века', NULL),
(N'Современное искусство', NULL),
(N'Портретная галерея', NULL),
(N'Скульптура Летнего сада', NULL),
(N'Интерьеры Строгановых', NULL),
(N'Тайны замка', NULL),
(N'Дар меценатов', NULL),
(N'Военная слава', NULL);
GO

INSERT INTO MuseumExhibition (MuseumID, ExhibitionID, StartDate, EndDate) VALUES
(10001, 10001, '2026-01-01', '2026-06-30'),
(10002, 10001, '2026-07-01', '2026-12-31'),
(10003, 10002, '2026-01-01', '2026-06-30'),
(10004, 10002, '2026-07-01', '2026-12-31'),
(10005, 10003, '2026-01-01', '2026-06-30'),
(10006, 10003, '2026-07-01', '2026-12-31'),
(10007, 10004, '2026-01-01', '2026-06-30'),
(10008, 10004, '2026-07-01', '2026-12-31'),
(10009, 10005, '2026-01-01', '2026-06-30'),
(10010, 10005, '2026-07-01', '2026-12-31'),
(10001, 10006, '2026-01-01', '2026-06-30'),
(10002, 10007, '2026-01-01', '2026-06-30'),
(10003, 10008, '2026-01-01', '2026-06-30'),
(10004, 10009, '2026-01-01', '2026-06-30'),
(10005, 10010, '2026-01-01', '2026-06-30'),
(10001, 10011, '2026-01-01', '2026-06-30'),
(10002, 10012, '2026-01-01', '2026-06-30'),
(10003, 10013, '2026-01-01', '2026-06-30'),
(10004, 10014, '2026-01-01', '2026-06-30'),
(10005, 10015, '2026-01-01', '2026-06-30'),
(10001, 10016, '2026-01-01', '2026-06-30'),
(10002, 10017, '2026-01-01', '2026-06-30'),
(10003, 10018, '2026-01-01', '2026-06-30'),
(10004, 10019, '2026-01-01', '2026-06-30'),
(10005, 10020, '2026-01-01', '2026-06-30');
GO


INSERT INTO Ticket (ExhibitionID, Type, Status, AvailableQuantity) VALUES
(10001, N'Взрослый', N'Доступен', 50),
(10001, N'Детский', N'Доступен', 30),
(10002, N'Взрослый', N'Доступен', 40),
(10002, N'Детский', N'Доступен', 20),
(10003, N'Взрослый', N'Доступен', 60),
(10003, N'Детский', N'Доступен', 35),
(10004, N'Взрослый', N'Доступен', 25),
(10004, N'Детский', N'Доступен', 15),
(10005, N'Взрослый', N'Доступен', 45),
(10005, N'Детский', N'Доступен', 25),
(10006, N'Взрослый', N'Доступен', 50),
(10006, N'Детский', N'Доступен', 30),
(10007, N'Взрослый', N'Доступен', 40),
(10007, N'Детский', N'Доступен', 20),
(10008, N'Взрослый', N'Доступен', 60),
(10008, N'Детский', N'Доступен', 35),
(10009, N'Взрослый', N'Доступен', 25),
(10009, N'Детский', N'Доступен', 15),
(10010, N'Взрослый', N'Доступен', 45),
(10010, N'Детский', N'Доступен', 25),
(10011, N'Взрослый', N'Доступен', 50),
(10012, N'Взрослый', N'Доступен', 50),
(10013, N'Взрослый', N'Доступен', 50),
(10014, N'Взрослый', N'Доступен', 50),
(10015, N'Взрослый', N'Доступен', 50),
(10016, N'Взрослый', N'Доступен', 50),
(10017, N'Взрослый', N'Доступен', 50),
(10018, N'Взрослый', N'Доступен', 50),
(10019, N'Взрослый', N'Доступен', 50),
(10020, N'Взрослый', N'Доступен', 50);
GO

INSERT INTO TicketPrice (TicketID, Price, StartDate, EndDate)
SELECT t.TicketID,
       CASE 
            WHEN t.ExhibitionID = 10001 THEN 500
            WHEN t.ExhibitionID = 10002 THEN 300
            WHEN t.ExhibitionID = 10003 THEN 450
            WHEN t.ExhibitionID = 10004 THEN 250
            WHEN t.ExhibitionID = 10005 THEN 600
            WHEN t.ExhibitionID >= 10006 AND t.ExhibitionID <= 10020 THEN 300
       END,
       '2026-01-01', '2026-12-31'
FROM Ticket t
WHERE t.Type = N'Взрослый';
GO

INSERT INTO TicketPrice (TicketID, Price, StartDate, EndDate)
SELECT t.TicketID,
       CASE 
            WHEN t.ExhibitionID = 10001 THEN 300
            WHEN t.ExhibitionID = 10002 THEN 200
            WHEN t.ExhibitionID = 10003 THEN 350
            WHEN t.ExhibitionID = 10004 THEN 150
            WHEN t.ExhibitionID = 10005 THEN 300
            WHEN t.ExhibitionID >= 10006 AND t.ExhibitionID <= 10020 THEN 200
       END,
       '2026-01-01', '2026-12-31'
FROM Ticket t
WHERE t.Type = N'Детский';
GO

INSERT INTO MuseumSchedule (MuseumID, StartDate, EndDate, OpenTime, CloseTime) VALUES
(10001, '2024-01-01', '2026-12-31', '10:00', '18:00'),
(10002, '2024-01-01', '2026-12-31', '11:00', '19:00');
GO

INSERT INTO ScheduleDays (MuseumScheduleID, WeekDay) VALUES
(10001, 1),
(10001, 2),
(10001, 3),
(10001, 4),
(10001, 5),
(10002, 1),
(10002, 2),
(10002, 3),
(10002, 4),
(10002, 5),
(10002, 6);
GO

INSERT INTO ScheduleExceptions (MuseumScheduleID, ExceptionDate, IsOpen, OpenTime, CloseTime) VALUES
(10001, '2026-03-08', 0, NULL, NULL),
(10001, '2026-03-10', 1, '10:00', '16:00'),
(10002, '2026-05-01', 0, NULL, NULL),
(10002, '2026-05-02', 1, '11:00', '15:00');
GO
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MuseumExhibition");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "ScheduleDays");

            migrationBuilder.DropTable(
                name: "ScheduleExceptions");

            migrationBuilder.DropTable(
                name: "TicketPrice");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "MuseumSchedule");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Museum");

            migrationBuilder.DropTable(
                name: "Exhibition");

            migrationBuilder.DropTable(
                name: "MuseumComplex");
        }
    }
}
