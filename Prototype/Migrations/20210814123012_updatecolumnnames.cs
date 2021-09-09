using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class updatecolumnnames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Jobs",
                newName: "LevelEnum");

            migrationBuilder.RenameColumn(
                name: "JobTitle",
                table: "Jobs",
                newName: "JobTitleEnum");

            migrationBuilder.RenameColumn(
                name: "JobTitle",
                table: "Candidates",
                newName: "JobTitleEnum");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LevelEnum",
                table: "Jobs",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "JobTitleEnum",
                table: "Jobs",
                newName: "JobTitle");

            migrationBuilder.RenameColumn(
                name: "JobTitleEnum",
                table: "Candidates",
                newName: "JobTitle");
        }
    }
}
