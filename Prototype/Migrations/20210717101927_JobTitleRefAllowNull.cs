using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class JobTitleRefAllowNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobTitle_JobTitleRefId",
                table: "Jobs");

            migrationBuilder.AlterColumn<int>(
                name: "JobTitleRefId",
                table: "Jobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobTitle_JobTitleRefId",
                table: "Jobs",
                column: "JobTitleRefId",
                principalTable: "JobTitle",
                principalColumn: "JobTitleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobTitle_JobTitleRefId",
                table: "Jobs");

            migrationBuilder.AlterColumn<int>(
                name: "JobTitleRefId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobTitle_JobTitleRefId",
                table: "Jobs",
                column: "JobTitleRefId",
                principalTable: "JobTitle",
                principalColumn: "JobTitleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
