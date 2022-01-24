using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleApplication.Data.Migrations
{
    public partial class RemveUserIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "EmployeeModel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CustomerModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "EmployeeModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CustomerModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
