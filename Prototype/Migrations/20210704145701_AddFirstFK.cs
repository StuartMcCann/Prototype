using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class AddFirstFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobTitleRefId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobTitleRefId",
                table: "Jobs",
                column: "JobTitleRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobTitles_JobTitleRefId",
                table: "Jobs",
                column: "JobTitleRefId",
                principalTable: "JobTitles",
                principalColumn: "JobTitleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobTitles_JobTitleRefId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobTitleRefId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobTitleRefId",
                table: "Jobs");
        }
    }
}
