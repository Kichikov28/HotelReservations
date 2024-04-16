using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelReservations.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientHistory_Clients_ClientId",
                table: "ClientHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientHistory",
                table: "ClientHistory");

            migrationBuilder.RenameTable(
                name: "ClientHistory",
                newName: "ClientHistories");

            migrationBuilder.RenameIndex(
                name: "IX_ClientHistory_ClientId",
                table: "ClientHistories",
                newName: "IX_ClientHistories_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientHistories",
                table: "ClientHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientHistories_Clients_ClientId",
                table: "ClientHistories",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientHistories_Clients_ClientId",
                table: "ClientHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientHistories",
                table: "ClientHistories");

            migrationBuilder.RenameTable(
                name: "ClientHistories",
                newName: "ClientHistory");

            migrationBuilder.RenameIndex(
                name: "IX_ClientHistories_ClientId",
                table: "ClientHistory",
                newName: "IX_ClientHistory_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientHistory",
                table: "ClientHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientHistory_Clients_ClientId",
                table: "ClientHistory",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
