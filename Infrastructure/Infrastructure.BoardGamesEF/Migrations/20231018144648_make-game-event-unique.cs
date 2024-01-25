using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.BoardGamesEF.Migrations
{
    /// <inheritdoc />
    public partial class makegameeventunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEventRegistrations_Users_UserId",
                table: "GameEventRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_GameEventRegistrations_UserId",
                table: "GameEventRegistrations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GameEventRegistrations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameEventRegistrations_UserId_GameEventId",
                table: "GameEventRegistrations",
                columns: new[] { "UserId", "GameEventId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameEventRegistrations_Users_UserId",
                table: "GameEventRegistrations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEventRegistrations_Users_UserId",
                table: "GameEventRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_GameEventRegistrations_UserId_GameEventId",
                table: "GameEventRegistrations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GameEventRegistrations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_GameEventRegistrations_UserId",
                table: "GameEventRegistrations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEventRegistrations_Users_UserId",
                table: "GameEventRegistrations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
