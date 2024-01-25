using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.BoardGamesEF.Migrations
{
    /// <inheritdoc />
    public partial class renameregistrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEventRegistrations_GameEvents_GameEventId",
                table: "GameEventRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_GameEventRegistrations_Users_UserId",
                table: "GameEventRegistrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameEventRegistrations",
                table: "GameEventRegistrations");

            migrationBuilder.RenameTable(
                name: "GameEventRegistrations",
                newName: "Registrations");

            migrationBuilder.RenameIndex(
                name: "IX_GameEventRegistrations_UserId_GameEventId",
                table: "Registrations",
                newName: "IX_Registrations_UserId_GameEventId");

            migrationBuilder.RenameIndex(
                name: "IX_GameEventRegistrations_GameEventId",
                table: "Registrations",
                newName: "IX_Registrations_GameEventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_GameEvents_GameEventId",
                table: "Registrations",
                column: "GameEventId",
                principalTable: "GameEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Users_UserId",
                table: "Registrations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_GameEvents_GameEventId",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Users_UserId",
                table: "Registrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations");

            migrationBuilder.RenameTable(
                name: "Registrations",
                newName: "GameEventRegistrations");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_UserId_GameEventId",
                table: "GameEventRegistrations",
                newName: "IX_GameEventRegistrations_UserId_GameEventId");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_GameEventId",
                table: "GameEventRegistrations",
                newName: "IX_GameEventRegistrations_GameEventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameEventRegistrations",
                table: "GameEventRegistrations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEventRegistrations_GameEvents_GameEventId",
                table: "GameEventRegistrations",
                column: "GameEventId",
                principalTable: "GameEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameEventRegistrations_Users_UserId",
                table: "GameEventRegistrations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
