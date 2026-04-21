using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Museum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToExhibitions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Exhibition",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Exhibition");
        }
    }
}
