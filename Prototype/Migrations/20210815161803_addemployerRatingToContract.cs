using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class addemployerRatingToContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContractRating",
                table: "Contracts",
                newName: "ContractRatingEmployer");

            migrationBuilder.AddColumn<int>(
                name: "ContractRatingCandidate",
                table: "Contracts",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractRatingCandidate",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "ContractRatingEmployer",
                table: "Contracts",
                newName: "ContractRating");
        }
    }
}
