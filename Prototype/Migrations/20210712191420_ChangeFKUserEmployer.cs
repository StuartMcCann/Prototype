using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class ChangeFKUserEmployer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employers_User_ApplicationUserId",
                table: "Employers");

            migrationBuilder.DropIndex(
                name: "IX_Employers_ApplicationUserId",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Employers");

            migrationBuilder.AddColumn<int>(
                name: "EmployerId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_EmployerId",
                table: "User",
                column: "EmployerId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Employers_EmployerId",
                table: "User",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "EmployerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Employers_EmployerId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_EmployerId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Employers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employers_ApplicationUserId",
                table: "Employers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employers_User_ApplicationUserId",
                table: "Employers",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
