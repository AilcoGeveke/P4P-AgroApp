using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AWA.Migrations
{
    public partial class TimespanToDateTimeInTimesheets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timesheets_EmployeeAssignments_EmployeeAssignmentId",
                table: "Timesheets");

            migrationBuilder.DropIndex(
                name: "IX_Timesheets_EmployeeAssignmentId",
                table: "Timesheets");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TotalTime",
                table: "Timesheets",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Timesheets",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Timesheets",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "TotalTime",
                table: "Timesheets",
                nullable: false);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "Timesheets",
                nullable: false);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "Timesheets",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Timesheets_EmployeeAssignmentId",
                table: "Timesheets",
                column: "EmployeeAssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timesheets_EmployeeAssignments_EmployeeAssignmentId",
                table: "Timesheets",
                column: "EmployeeAssignmentId",
                principalTable: "EmployeeAssignments",
                principalColumn: "EmployeeAssignmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
