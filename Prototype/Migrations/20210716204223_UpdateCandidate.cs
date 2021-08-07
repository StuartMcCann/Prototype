using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class UpdateCandidate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Candidates");

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableFrom",
                table: "Candidates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Candidates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableFrom",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Candidates");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "Candidates",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
