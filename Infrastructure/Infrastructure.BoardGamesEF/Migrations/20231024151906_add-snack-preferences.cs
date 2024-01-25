using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.BoardGamesEF.Migrations
{
    /// <inheritdoc />
    public partial class addsnackpreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SnackPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnackPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameEventSnackPreference",
                columns: table => new
                {
                    AvailableSnacksId = table.Column<int>(type: "int", nullable: false),
                    GameEventsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEventSnackPreference", x => new { x.AvailableSnacksId, x.GameEventsId });
                    table.ForeignKey(
                        name: "FK_GameEventSnackPreference_GameEvents_GameEventsId",
                        column: x => x.GameEventsId,
                        principalTable: "GameEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameEventSnackPreference_SnackPreferences_AvailableSnacksId",
                        column: x => x.AvailableSnacksId,
                        principalTable: "SnackPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SnackPreferenceUser",
                columns: table => new
                {
                    SnackPreferencesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnackPreferenceUser", x => new { x.SnackPreferencesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SnackPreferenceUser_SnackPreferences_SnackPreferencesId",
                        column: x => x.SnackPreferencesId,
                        principalTable: "SnackPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnackPreferenceUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameEventSnackPreference_GameEventsId",
                table: "GameEventSnackPreference",
                column: "GameEventsId");

            migrationBuilder.CreateIndex(
                name: "IX_SnackPreferenceUser_UsersId",
                table: "SnackPreferenceUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameEventSnackPreference");

            migrationBuilder.DropTable(
                name: "SnackPreferenceUser");

            migrationBuilder.DropTable(
                name: "SnackPreferences");
        }
    }
}
