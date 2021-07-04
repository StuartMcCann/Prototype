using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class AddFKJobEmployer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployerId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployerRefId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_EmployerId",
                table: "Jobs",
                column: "EmployerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Employers_EmployerId",
                table: "Jobs",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "EmployerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Employers_EmployerId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_EmployerId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "EmployerRefId",
                table: "Jobs");
        }
    }
}
