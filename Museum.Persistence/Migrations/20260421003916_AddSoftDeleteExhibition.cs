using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Museum.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteExhibition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Ticket_TicketID",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Exhibition_ExhibitionID",
                table: "Ticket");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Ticket_TicketID",
                table: "OrderItem",
                column: "TicketID",
                principalTable: "Ticket",
                principalColumn: "TicketID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Exhibition_ExhibitionID",
                table: "Ticket",
                column: "ExhibitionID",
                principalTable: "Exhibition",
                principalColumn: "ExhibitionID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Ticket_TicketID",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Exhibition_ExhibitionID",
                table: "Ticket");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Ticket_TicketID",
                table: "OrderItem",
                column: "TicketID",
                principalTable: "Ticket",
                principalColumn: "TicketID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Exhibition_ExhibitionID",
                table: "Ticket",
                column: "ExhibitionID",
                principalTable: "Exhibition",
                principalColumn: "ExhibitionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
