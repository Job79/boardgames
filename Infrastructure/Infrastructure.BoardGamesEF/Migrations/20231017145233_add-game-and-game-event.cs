using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.BoardGamesEF.Migrations
{
    /// <inheritdoc />
    public partial class addgameandgameevent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "GameEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "GameEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "GameEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "GameEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaxPlayers",
                table: "GameEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "GameEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "GameEvents");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "GameEvents");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "GameEvents");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "GameEvents");

            migrationBuilder.DropColumn(
                name: "MaxPlayers",
                table: "GameEvents");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "GameEvents");
        }
    }
}
