using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.BoardGamesEF.Migrations
{
    /// <inheritdoc />
    public partial class addgamelisttogameevent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameGameEvent",
                columns: table => new
                {
                    GameEventsId = table.Column<int>(type: "int", nullable: false),
                    GamesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGameEvent", x => new { x.GameEventsId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_GameGameEvent_GameEvents_GameEventsId",
                        column: x => x.GameEventsId,
                        principalTable: "GameEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGameEvent_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameGameEvent_GamesId",
                table: "GameGameEvent",
                column: "GamesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGameEvent");
        }
    }
}
