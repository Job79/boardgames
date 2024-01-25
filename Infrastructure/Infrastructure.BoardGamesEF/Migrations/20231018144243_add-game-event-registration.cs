using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.BoardGamesEF.Migrations
{
    /// <inheritdoc />
    public partial class addgameeventregistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEventRegistration_GameEvents_GameEventId",
                table: "GameEventRegistration");

            migrationBuilder.DropForeignKey(
                name: "FK_GameEventRegistration_Users_UserId",
                table: "GameEventRegistration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameEventRegistration",
                table: "GameEventRegistration");

            migrationBuilder.RenameTable(
                name: "GameEventRegistration",
                newName: "GameEventRegistrations");

            migrationBuilder.RenameIndex(
                name: "IX_GameEventRegistration_UserId",
                table: "GameEventRegistrations",
                newName: "IX_GameEventRegistrations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GameEventRegistration_GameEventId",
                table: "GameEventRegistrations",
                newName: "IX_GameEventRegistrations_GameEventId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GameEventRegistrations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "GameEventRegistration");

            migrationBuilder.RenameIndex(
                name: "IX_GameEventRegistrations_UserId",
                table: "GameEventRegistration",
                newName: "IX_GameEventRegistration_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GameEventRegistrations_GameEventId",
                table: "GameEventRegistration",
                newName: "IX_GameEventRegistration_GameEventId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GameEventRegistration",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameEventRegistration",
                table: "GameEventRegistration",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEventRegistration_GameEvents_GameEventId",
                table: "GameEventRegistration",
                column: "GameEventId",
                principalTable: "GameEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameEventRegistration_Users_UserId",
                table: "GameEventRegistration",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
