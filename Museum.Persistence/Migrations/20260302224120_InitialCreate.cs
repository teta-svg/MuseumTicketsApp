using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Museum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exhibition",
                columns: table => new
                {
                    ExhibitionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    TicketID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExhibitionID = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.TicketID);
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderID);
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPrice", x => x.TicketPriceID);
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MuseumID = table.Column<int>(type: "int", nullable: false),
                    ExhibitionID = table.Column<int>(type: "int", nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.OrderItemID);
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentID);
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MuseumScheduleID = table.Column<int>(type: "int", nullable: false),
                    WeekDay = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDays", x => x.ScheduleDaysID);
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                table: "Exhibition",
                columns: new[] { "ExhibitionID", "EndDate", "Name", "Photo", "StartDate" },
                values: new object[,]
                {
                    { 10001, new DateOnly(2026, 12, 31), "Шедевры Рембрандта", null, new DateOnly(2024, 1, 1) },
                    { 10002, new DateOnly(2026, 12, 31), "Золото скифов", null, new DateOnly(2024, 1, 1) },
                    { 10003, new DateOnly(2026, 12, 31), "Импрессионисты", null, new DateOnly(2024, 1, 1) },
                    { 10004, new DateOnly(2026, 12, 31), "Искусство Востока", null, new DateOnly(2024, 1, 1) },
                    { 10005, new DateOnly(2026, 12, 31), "Петровская эпоха", null, new DateOnly(2024, 1, 1) },
                    { 10006, new DateOnly(2026, 12, 31), "История гвардии", null, new DateOnly(2024, 1, 1) },
                    { 10007, new DateOnly(2026, 12, 31), "Архитектура барокко", null, new DateOnly(2024, 1, 1) },
                    { 10008, new DateOnly(2026, 12, 31), "Быт аристократии", null, new DateOnly(2024, 1, 1) },
                    { 10009, new DateOnly(2026, 12, 31), "Фарфоровая сказка", null, new DateOnly(2024, 1, 1) },
                    { 10010, new DateOnly(2026, 12, 31), "Техника росписи", null, new DateOnly(2024, 1, 1) },
                    { 10011, new DateOnly(2026, 12, 31), "Русская икона", null, new DateOnly(2024, 1, 1) },
                    { 10012, new DateOnly(2026, 12, 31), "Передвижники", null, new DateOnly(2024, 1, 1) },
                    { 10013, new DateOnly(2026, 12, 31), "Авангард XX века", null, new DateOnly(2024, 1, 1) },
                    { 10014, new DateOnly(2026, 12, 31), "Современное искусство", null, new DateOnly(2024, 1, 1) },
                    { 10015, new DateOnly(2026, 12, 31), "Портретная галерея", null, new DateOnly(2024, 1, 1) },
                    { 10016, new DateOnly(2026, 12, 31), "Скульптура Летнего сада", null, new DateOnly(2024, 1, 1) },
                    { 10017, new DateOnly(2026, 12, 31), "Интерьеры Строгановых", null, new DateOnly(2024, 1, 1) },
                    { 10018, new DateOnly(2026, 12, 31), "Тайны замка", null, new DateOnly(2024, 1, 1) },
                    { 10019, new DateOnly(2026, 12, 31), "Дар меценатов", null, new DateOnly(2024, 1, 1) },
                    { 10020, new DateOnly(2026, 12, 31), "Военная слава", null, new DateOnly(2024, 1, 1) }
                });

            migrationBuilder.InsertData(
                table: "MuseumComplex",
                columns: new[] { "MuseumComplexID", "Name" },
                values: new object[,]
                {
                    { 10001, "Государственный Эрмитаж" },
                    { 10002, "Государственный Русский музей" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserID", "Email", "FirstName", "LastName", "MiddleName", "Password", "Phone", "Role" },
                values: new object[] { 10001, "admin@museum.ru", "Петр", "Админов", "Петрович", "$2a$11$0NR8Ir7eg9MNV9VFQsudROSeSoRodME7UsMzNNfmLla/e/gLzQyuK", "+70000000000", "Администратор системы" });

            migrationBuilder.InsertData(
                table: "Museum",
                columns: new[] { "MuseumID", "City", "House", "MuseumComplexID", "Name", "Street" },
                values: new object[,]
                {
                    { 10001, "Санкт-Петербург", "34", 10001, "Зимний дворец", "Дворцовая наб." },
                    { 10002, "Санкт-Петербург", "6", 10001, "Главный штаб", "Дворцовая пл." },
                    { 10003, "Санкт-Петербург", "32", 10001, "Зимний дворец Петра I", "Дворцовая наб." },
                    { 10004, "Санкт-Петербург", "15", 10001, "Дворец Меншикова", "Университетская наб." },
                    { 10005, "Санкт-Петербург", "151", 10001, "Музей фарфора", "пр. Обуховской Обороны" },
                    { 10006, "Санкт-Петербург", "4", 10002, "Михайловский дворец", "Инженерная ул." },
                    { 10007, "Санкт-Петербург", "2", 10002, "Корпус Бенуа", "наб. канала Грибоедова" },
                    { 10008, "Санкт-Петербург", "5", 10002, "Мраморный дворец", "Миллионная ул." },
                    { 10009, "Санкт-Петербург", "17", 10002, "Строгановский дворец", "Невский пр." },
                    { 10010, "Санкт-Петербург", "2", 10002, "Михайловский замок", "Садовая ул." }
                });

            migrationBuilder.InsertData(
                table: "Ticket",
                columns: new[] { "TicketID", "AvailableQuantity", "ExhibitionID", "Status", "Type" },
                values: new object[,]
                {
                    { 10001, 100, 10001, "Доступен", "Взрослый" },
                    { 10002, 50, 10001, "Доступен", "Льготный" },
                    { 10003, 100, 10002, "Доступен", "Взрослый" },
                    { 10004, 50, 10002, "Доступен", "Льготный" },
                    { 10005, 100, 10003, "Доступен", "Взрослый" },
                    { 10006, 50, 10003, "Доступен", "Льготный" },
                    { 10007, 100, 10004, "Доступен", "Взрослый" },
                    { 10008, 50, 10004, "Доступен", "Льготный" },
                    { 10009, 100, 10005, "Доступен", "Взрослый" },
                    { 10010, 50, 10005, "Доступен", "Льготный" },
                    { 10011, 100, 10006, "Доступен", "Взрослый" },
                    { 10012, 50, 10006, "Доступен", "Льготный" },
                    { 10013, 100, 10007, "Доступен", "Взрослый" },
                    { 10014, 50, 10007, "Доступен", "Льготный" },
                    { 10015, 100, 10008, "Доступен", "Взрослый" },
                    { 10016, 50, 10008, "Доступен", "Льготный" },
                    { 10017, 100, 10009, "Доступен", "Взрослый" },
                    { 10018, 50, 10009, "Доступен", "Льготный" },
                    { 10019, 100, 10010, "Доступен", "Взрослый" },
                    { 10020, 50, 10010, "Доступен", "Льготный" },
                    { 10021, 100, 10011, "Доступен", "Взрослый" },
                    { 10022, 50, 10011, "Доступен", "Льготный" },
                    { 10023, 100, 10012, "Доступен", "Взрослый" },
                    { 10024, 50, 10012, "Доступен", "Льготный" },
                    { 10025, 100, 10013, "Доступен", "Взрослый" },
                    { 10026, 50, 10013, "Доступен", "Льготный" },
                    { 10027, 100, 10014, "Доступен", "Взрослый" },
                    { 10028, 50, 10014, "Доступен", "Льготный" },
                    { 10029, 100, 10015, "Доступен", "Взрослый" },
                    { 10030, 50, 10015, "Доступен", "Льготный" },
                    { 10031, 100, 10016, "Доступен", "Взрослый" },
                    { 10032, 50, 10016, "Доступен", "Льготный" },
                    { 10033, 100, 10017, "Доступен", "Взрослый" },
                    { 10034, 50, 10017, "Доступен", "Льготный" },
                    { 10035, 100, 10018, "Доступен", "Взрослый" },
                    { 10036, 50, 10018, "Доступен", "Льготный" },
                    { 10037, 100, 10019, "Доступен", "Взрослый" },
                    { 10038, 50, 10019, "Доступен", "Льготный" },
                    { 10039, 100, 10020, "Доступен", "Взрослый" },
                    { 10040, 50, 10020, "Доступен", "Льготный" }
                });

            migrationBuilder.InsertData(
                table: "TicketPrice",
                columns: new[] { "TicketPriceID", "Price", "TicketID" },
                values: new object[,]
                {
                    { 10001, 700.0m, 10001 },
                    { 10002, 350.0m, 10002 },
                    { 10003, 600.0m, 10003 },
                    { 10004, 300.0m, 10004 },
                    { 10005, 500.0m, 10005 },
                    { 10006, 250.0m, 10006 },
                    { 10007, 800.0m, 10007 },
                    { 10008, 400.0m, 10008 },
                    { 10009, 550.0m, 10009 },
                    { 10010, 275.0m, 10010 },
                    { 10011, 400.0m, 10011 },
                    { 10012, 200.0m, 10012 },
                    { 10013, 650.0m, 10013 },
                    { 10014, 325.0m, 10014 },
                    { 10015, 450.0m, 10015 },
                    { 10016, 225.0m, 10016 },
                    { 10017, 300.0m, 10017 },
                    { 10018, 150.0m, 10018 },
                    { 10019, 250.0m, 10019 },
                    { 10020, 125.0m, 10020 },
                    { 10021, 500.0m, 10021 },
                    { 10022, 250.0m, 10022 },
                    { 10023, 600.0m, 10023 },
                    { 10024, 300.0m, 10024 },
                    { 10025, 750.0m, 10025 },
                    { 10026, 375.0m, 10026 },
                    { 10027, 400.0m, 10027 },
                    { 10028, 200.0m, 10028 },
                    { 10029, 350.0m, 10029 },
                    { 10030, 175.0m, 10030 },
                    { 10031, 450.0m, 10031 },
                    { 10032, 225.0m, 10032 },
                    { 10033, 550.0m, 10033 },
                    { 10034, 275.0m, 10034 },
                    { 10035, 650.0m, 10035 },
                    { 10036, 325.0m, 10036 },
                    { 10037, 400.0m, 10037 },
                    { 10038, 200.0m, 10038 },
                    { 10039, 500.0m, 10039 },
                    { 10040, 250.0m, 10040 }
                });

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
