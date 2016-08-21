using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AWA.Migrations
{
    public partial class RelationFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeAssignments",
                table: "EmployeeAssignments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeAssignments",
                table: "EmployeeAssignments",
                column: "EmployeeAssignmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeAssignments",
                table: "EmployeeAssignments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeAssignments",
                table: "EmployeeAssignments",
                columns: new[] { "AssignmentId", "UserId" });
        }
    }
}
