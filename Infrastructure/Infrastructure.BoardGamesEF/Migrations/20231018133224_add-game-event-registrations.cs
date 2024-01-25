using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.BoardGamesEF.Migrations
{
    /// <inheritdoc />
    public partial class addgameeventregistrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEvents_Users_OrganizerId",
                table: "GameEvents");

            migrationBuilder.DropTable(
                name: "GameEventUser");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizerId",
                table: "GameEvents",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "GameEventRegistration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GameEventId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEventRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameEventRegistration_GameEvents_GameEventId",
                        column: x => x.GameEventId,
                        principalTable: "GameEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameEventRegistration_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genre = table.Column<int>(type: "int", nullable: false),
                    Is18Plus = table.Column<bool>(type: "bit", nullable: false),
                    ImageUri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "Description", "Genre", "ImageUri", "Is18Plus", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Het populaire handelsspel Catan in een nieuw jasje! Nu met nog meer spelbeleving! Lukt het jou om op Catan de belangrijkste macht te worden?", 0, "https://image.intertoys.nl/wcsstore/IntertoysCAS/images/catalog/full/1006506-e69938b2.jpg", false, "Catan", 0 },
                    { 2, "Speel Monopoly Classic en maak kennis met de badeend, de Tyrannosaurus Rex en de pinguïn. Ga kopen en onderhandelen om de ultieme rijkdom te behalen.", 2, "https://image.intertoys.nl/wcsstore/IntertoysCAS/images/catalog/full/1557023-b23d0603.jpg", false, "Monopoly", 1 },
                    { 3, "Verover het territorium van je vijanden in het strategische spel Risk! De worp van de dobbelstenen is jouw weg naar de overwinning!", 0, "https://image.intertoys.nl/wcsstore/IntertoysCAS/images/catalog/full/1387810-9348cdd6.jpg", false, "Risk", 1 },
                    { 4, "Een speciale editie van Rummikub in een luxe, zwart blik! Ter ere van het 70-jarig bestaan van Rummikub heeft deze versie zware, zwarte stenen!", 4, "https://image.intertoys.nl/wcsstore/IntertoysCAS/images/catalog/thumb/1984456-04e5b2e1.jpg", true, "Rummikub", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameEventRegistration_GameEventId",
                table: "GameEventRegistration",
                column: "GameEventId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEventRegistration_UserId",
                table: "GameEventRegistration",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEvents_Users_OrganizerId",
                table: "GameEvents",
                column: "OrganizerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameEvents_Users_OrganizerId",
                table: "GameEvents");

            migrationBuilder.DropTable(
                name: "GameEventRegistration");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizerId",
                table: "GameEvents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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
                name: "IX_GameEventUser_PlayersId",
                table: "GameEventUser",
                column: "PlayersId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameEvents_Users_OrganizerId",
                table: "GameEvents",
                column: "OrganizerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
