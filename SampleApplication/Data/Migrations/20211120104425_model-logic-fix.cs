using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleApplication.Data.Migrations
{
    public partial class modellogicfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceModel_EmployeeModel_EmployeeModelId",
                table: "ServiceModel");

            migrationBuilder.DropIndex(
                name: "IX_ServiceModel_EmployeeModelId",
                table: "ServiceModel");

            migrationBuilder.DropColumn(
                name: "EmployeeModelId",
                table: "ServiceModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeModelId",
                table: "ServiceModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceModel_EmployeeModelId",
                table: "ServiceModel",
                column: "EmployeeModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceModel_EmployeeModel_EmployeeModelId",
                table: "ServiceModel",
                column: "EmployeeModelId",
                principalTable: "EmployeeModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
