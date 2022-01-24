using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleApplication.Data.Migrations
{
    public partial class EmployeeUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeModel_AspNetUsers_UserId",
                table: "EmployeeModel");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EmployeeModel",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeModel_AspNetUsers_UserId",
                table: "EmployeeModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeModel_AspNetUsers_UserId",
                table: "EmployeeModel");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EmployeeModel",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeModel_AspNetUsers_UserId",
                table: "EmployeeModel",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
