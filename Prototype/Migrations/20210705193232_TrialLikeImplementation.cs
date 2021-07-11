using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class TrialLikeImplementation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CandidateEmployer",
                columns: table => new
                {
                    CandidatesCandidateId = table.Column<int>(type: "int", nullable: false),
                    EmployersEmployerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateEmployer", x => new { x.CandidatesCandidateId, x.EmployersEmployerId });
                    table.ForeignKey(
                        name: "FK_CandidateEmployer_Candidates_CandidatesCandidateId",
                        column: x => x.CandidatesCandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateEmployer_Employers_EmployersEmployerId",
                        column: x => x.EmployersEmployerId,
                        principalTable: "Employers",
                        principalColumn: "EmployerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateJob",
                columns: table => new
                {
                    CandidatesCandidateId = table.Column<int>(type: "int", nullable: false),
                    JobsJobId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateJob", x => new { x.CandidatesCandidateId, x.JobsJobId });
                    table.ForeignKey(
                        name: "FK_CandidateJob_Candidates_CandidatesCandidateId",
                        column: x => x.CandidatesCandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateJob_Jobs_JobsJobId",
                        column: x => x.JobsJobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateEmployer_EmployersEmployerId",
                table: "CandidateEmployer",
                column: "EmployersEmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateJob_JobsJobId",
                table: "CandidateJob",
                column: "JobsJobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateEmployer");

            migrationBuilder.DropTable(
                name: "CandidateJob");
        }
    }
}
