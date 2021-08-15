using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class israted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRated",
                table: "Contracts",
                newName: "IsRatedEmployer");

            migrationBuilder.AddColumn<bool>(
                name: "IsRatedCandidate",
                table: "Contracts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRatedCandidate",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "IsRatedEmployer",
                table: "Contracts",
                newName: "IsRated");
        }
    }
}
