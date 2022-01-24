using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleApplication.Data.Migrations
{
    public partial class AdjustESModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "EmployeeServiceModel");

            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "EmployeeServiceModel");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "EmployeeServiceModel",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "EmployeeServiceModel",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeServiceModel_EmployeeId",
                table: "EmployeeServiceModel",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeServiceModel_ServiceId",
                table: "EmployeeServiceModel",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeServiceModel_EmployeeModel_EmployeeId",
                table: "EmployeeServiceModel",
                column: "EmployeeId",
                principalTable: "EmployeeModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeServiceModel_ServiceModel_ServiceId",
                table: "EmployeeServiceModel",
                column: "ServiceId",
                principalTable: "ServiceModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeServiceModel_EmployeeModel_EmployeeId",
                table: "EmployeeServiceModel");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeServiceModel_ServiceModel_ServiceId",
                table: "EmployeeServiceModel");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeServiceModel_EmployeeId",
                table: "EmployeeServiceModel");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeServiceModel_ServiceId",
                table: "EmployeeServiceModel");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "EmployeeServiceModel",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "EmployeeServiceModel",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "EmployeeServiceModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "EmployeeServiceModel",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
