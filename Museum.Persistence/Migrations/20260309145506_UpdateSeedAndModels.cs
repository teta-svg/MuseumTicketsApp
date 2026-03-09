using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Museum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedAndModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Exhibition");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Exhibition");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "MuseumExhibition",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "MuseumExhibition",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10023,
                column: "Price",
                value: 400m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10024,
                column: "Price",
                value: 200m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10025,
                column: "Price",
                value: 350m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10026,
                column: "Price",
                value: 175m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10027,
                column: "Price",
                value: 450m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10028,
                column: "Price",
                value: 225m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10029,
                column: "Price",
                value: 400m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10030,
                column: "Price",
                value: 200m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10031,
                column: "Price",
                value: 300m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10032,
                column: "Price",
                value: 150m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10033,
                column: "Price",
                value: 250m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10034,
                column: "Price",
                value: 125m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10035,
                column: "Price",
                value: 500m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10036,
                column: "Price",
                value: 250m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10037,
                column: "Price",
                value: 450m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10038,
                column: "Price",
                value: 225m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10039,
                column: "Price",
                value: 600m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10040,
                column: "Price",
                value: 300m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "MuseumExhibition");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "MuseumExhibition");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "Exhibition",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "Exhibition",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10001,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10002,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10003,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10004,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10005,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10006,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10007,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10008,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10009,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10010,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10011,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10012,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10013,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10014,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10015,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10016,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10017,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10018,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10019,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "Exhibition",
                keyColumn: "ExhibitionID",
                keyValue: 10020,
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 12, 31), new DateOnly(2024, 1, 1) });

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10023,
                column: "Price",
                value: 600.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10024,
                column: "Price",
                value: 300.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10025,
                column: "Price",
                value: 750.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10026,
                column: "Price",
                value: 375.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10027,
                column: "Price",
                value: 400.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10028,
                column: "Price",
                value: 200.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10029,
                column: "Price",
                value: 350.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10030,
                column: "Price",
                value: 175.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10031,
                column: "Price",
                value: 450.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10032,
                column: "Price",
                value: 225.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10033,
                column: "Price",
                value: 550.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10034,
                column: "Price",
                value: 275.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10035,
                column: "Price",
                value: 650.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10036,
                column: "Price",
                value: 325.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10037,
                column: "Price",
                value: 400.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10038,
                column: "Price",
                value: 200.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10039,
                column: "Price",
                value: 500.0m);

            migrationBuilder.UpdateData(
                table: "TicketPrice",
                keyColumn: "TicketPriceID",
                keyValue: 10040,
                column: "Price",
                value: 250.0m);
        }
    }
}
