using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleApplication.Data.Migrations
{
    public partial class ModelAdjustment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "EmployeeServiceModel");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ServiceModel",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ServiceModel");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "EmployeeServiceModel",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
