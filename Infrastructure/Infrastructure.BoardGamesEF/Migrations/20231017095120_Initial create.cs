using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.BoardGamesEF.Migrations
{
    /// <inheritdoc />
    public partial class Initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameEvents_Users_OrganizerId",
                        column: x => x.OrganizerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameEventUser",
                columns: table => new
                {
                    ParticipatingGameEventsId = table.Column<int>(type: "int", nullable: false),
                    PlayersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEventUser", x => new { x.ParticipatingGameEventsId, x.PlayersId });
                    table.ForeignKey(
                        name: "FK_GameEventUser_GameEvents_ParticipatingGameEventsId",
                        column: x => x.ParticipatingGameEventsId,
                        principalTable: "GameEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameEventUser_Users_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameEvents_OrganizerId",
                table: "GameEvents",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEventUser_PlayersId",
                table: "GameEventUser",
                column: "PlayersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameEventUser");

            migrationBuilder.DropTable(
                name: "GameEvents");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
