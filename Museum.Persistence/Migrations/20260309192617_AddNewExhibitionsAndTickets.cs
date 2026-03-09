using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Museum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewExhibitionsAndTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Museum_MuseumComplex",
                table: "Museum");

            migrationBuilder.DropForeignKey(
                name: "FK_MuseumExhibition_Exhibition",
                table: "MuseumExhibition");

            migrationBuilder.DropForeignKey(
                name: "FK_MuseumExhibition_Museum",
                table: "MuseumExhibition");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Order__C3905BAFC6F99E8E",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK__MuseumEx__8C9BE304C8B2B2E5",
                table: "MuseumExhibition");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Museum__C10D28D294428B64",
                table: "Museum");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Order",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "OrderID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MuseumExhibition",
                table: "MuseumExhibition",
                column: "MuseumExhibitionID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Museum",
                table: "Museum",
                column: "MuseumID");

            migrationBuilder.AddForeignKey(
                name: "FK_Museum_MuseumComplex_MuseumComplexID",
                table: "Museum",
                column: "MuseumComplexID",
                principalTable: "MuseumComplex",
                principalColumn: "MuseumComplexID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MuseumExhibition_Exhibition_ExhibitionID",
                table: "MuseumExhibition",
                column: "ExhibitionID",
                principalTable: "Exhibition",
                principalColumn: "ExhibitionID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MuseumExhibition_Museum_MuseumID",
                table: "MuseumExhibition",
                column: "MuseumID",
                principalTable: "Museum",
                principalColumn: "MuseumID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_UserID",
                table: "Order",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Museum_MuseumComplex_MuseumComplexID",
                table: "Museum");

            migrationBuilder.DropForeignKey(
                name: "FK_MuseumExhibition_Exhibition_ExhibitionID",
                table: "MuseumExhibition");

            migrationBuilder.DropForeignKey(
                name: "FK_MuseumExhibition_Museum_MuseumID",
                table: "MuseumExhibition");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_UserID",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MuseumExhibition",
                table: "MuseumExhibition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Museum",
                table: "Museum");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Order",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Order__C3905BAFC6F99E8E",
                table: "Order",
                column: "OrderID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__MuseumEx__8C9BE304C8B2B2E5",
                table: "MuseumExhibition",
                column: "MuseumExhibitionID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Museum__C10D28D294428B64",
                table: "Museum",
                column: "MuseumID");

            migrationBuilder.AddForeignKey(
                name: "FK_Museum_MuseumComplex",
                table: "Museum",
                column: "MuseumComplexID",
                principalTable: "MuseumComplex",
                principalColumn: "MuseumComplexID");

            migrationBuilder.AddForeignKey(
                name: "FK_MuseumExhibition_Exhibition",
                table: "MuseumExhibition",
                column: "ExhibitionID",
                principalTable: "Exhibition",
                principalColumn: "ExhibitionID");

            migrationBuilder.AddForeignKey(
                name: "FK_MuseumExhibition_Museum",
                table: "MuseumExhibition",
                column: "MuseumID",
                principalTable: "Museum",
                principalColumn: "MuseumID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User",
                table: "Order",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID");
        }
    }
}
