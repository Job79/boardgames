using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.BoardGamesEF.Migrations
{
    /// <inheritdoc />
    public partial class add16plustogameevent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is18Plus",
                table: "GameEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is18Plus",
                table: "GameEvents");
        }
    }
}
