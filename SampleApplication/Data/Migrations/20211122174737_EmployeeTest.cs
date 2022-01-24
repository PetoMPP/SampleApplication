using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleApplication.Data.Migrations
{
    public partial class EmployeeTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "EmployeeModel");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "EmployeeModel",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeModel_UserId",
                table: "EmployeeModel",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeModel_AspNetUsers_UserId",
                table: "EmployeeModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeModel_AspNetUsers_UserId",
                table: "EmployeeModel");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeModel_UserId",
                table: "EmployeeModel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmployeeModel");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "EmployeeModel",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");
        }
    }
}
