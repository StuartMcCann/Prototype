using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class AddReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobTitles_JobTitleRefId",
                table: "Jobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobTitles",
                table: "JobTitles");

            migrationBuilder.RenameTable(
                name: "JobTitles",
                newName: "JobTitle");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobTitle",
                table: "JobTitle",
                column: "JobTitleId");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    ReviewText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CandidateRefId = table.Column<int>(type: "int", nullable: false),
                    CandidateId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CandidateId",
                table: "Reviews",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobTitle_JobTitleRefId",
                table: "Jobs",
                column: "JobTitleRefId",
                principalTable: "JobTitle",
                principalColumn: "JobTitleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobTitle_JobTitleRefId",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobTitle",
                table: "JobTitle");

            migrationBuilder.RenameTable(
                name: "JobTitle",
                newName: "JobTitles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobTitles",
                table: "JobTitles",
                column: "JobTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobTitles_JobTitleRefId",
                table: "Jobs",
                column: "JobTitleRefId",
                principalTable: "JobTitles",
                principalColumn: "JobTitleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
