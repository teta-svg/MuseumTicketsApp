using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Museum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdmins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 10001,
                columns: new[] { "Email", "Phone" },
                values: new object[] { "sysadmin@museum.ru", "+70000000001" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserID", "Email", "FirstName", "LastName", "MiddleName", "Password", "Phone", "Role" },
                values: new object[] { 10002, "museumadmin@museum.ru", "Иван", "Музеев", "Иванович", "$2a$11$0NR8Ir7eg9MNV9VFQsudROSeSoRodME7UsMzNNfmLla/e/gLzQyuK", "+70000000002", "Администратор музея" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 10002);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "UserID",
                keyValue: 10001,
                columns: new[] { "Email", "Phone" },
                values: new object[] { "admin@museum.ru", "+70000000000" });
        }
    }
}
