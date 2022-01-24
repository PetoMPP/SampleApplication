using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleApplication.Data.Migrations
{
    public partial class RemoveSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShiftEnd",
                table: "EmployeeModel");

            migrationBuilder.DropColumn(
                name: "ShiftStart",
                table: "EmployeeModel");

            migrationBuilder.RenameColumn(
                name: "UserDbId",
                table: "EmployeeModel",
                newName: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "EmployeeModel",
                newName: "UserDbId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ShiftEnd",
                table: "EmployeeModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ShiftStart",
                table: "EmployeeModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
