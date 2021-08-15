using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class RatingNameRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRatedEmployer",
                table: "Contracts",
                newName: "IsRatedByEmployer");

            migrationBuilder.RenameColumn(
                name: "IsRatedCandidate",
                table: "Contracts",
                newName: "IsRatedByCandidate");

            migrationBuilder.RenameColumn(
                name: "ContractRatingEmployer",
                table: "Contracts",
                newName: "ContractRatingByEmployer");

            migrationBuilder.RenameColumn(
                name: "ContractRatingCandidate",
                table: "Contracts",
                newName: "ContractRatingByCandidate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRatedByEmployer",
                table: "Contracts",
                newName: "IsRatedEmployer");

            migrationBuilder.RenameColumn(
                name: "IsRatedByCandidate",
                table: "Contracts",
                newName: "IsRatedCandidate");

            migrationBuilder.RenameColumn(
                name: "ContractRatingByEmployer",
                table: "Contracts",
                newName: "ContractRatingEmployer");

            migrationBuilder.RenameColumn(
                name: "ContractRatingByCandidate",
                table: "Contracts",
                newName: "ContractRatingCandidate");
        }
    }
}
