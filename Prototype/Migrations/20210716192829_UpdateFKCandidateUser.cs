using Microsoft.EntityFrameworkCore.Migrations;

namespace Prototype.Migrations
{
    public partial class UpdateFKCandidateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_User_ApplicationUserId",
                table: "Candidates");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Candidates",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Candidates_ApplicationUserId",
                table: "Candidates",
                newName: "IX_Candidates_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_User_UserId",
                table: "Candidates",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_User_UserId",
                table: "Candidates");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Candidates",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Candidates_UserId",
                table: "Candidates",
                newName: "IX_Candidates_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_User_ApplicationUserId",
                table: "Candidates",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
