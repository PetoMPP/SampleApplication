using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleApplication.Data.Migrations
{
    public partial class AreThreAnyChangs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserDbId",
                table: "EmployeeModel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserDbId",
                table: "EmployeeModel");
        }
    }
}
