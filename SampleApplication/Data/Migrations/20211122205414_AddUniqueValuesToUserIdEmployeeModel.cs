using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleApplication.Data.Migrations
{
    public partial class AddUniqueValuesToUserIdEmployeeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeModel_UserId",
                table: "EmployeeModel");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeModel_UserId",
                table: "EmployeeModel",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeModel_UserId",
                table: "EmployeeModel");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeModel_UserId",
                table: "EmployeeModel",
                column: "UserId");
        }
    }
}
