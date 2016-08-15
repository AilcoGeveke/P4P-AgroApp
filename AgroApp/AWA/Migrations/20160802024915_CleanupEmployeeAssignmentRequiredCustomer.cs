﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AWA.Migrations
{
    public partial class CleanupEmployeeAssignmentRequiredCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAssignments_Assignments_AssignmentId",
                table: "EmployeeAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAssignments_Users_UserId",
                table: "EmployeeAssignments");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "EmployeeAssignments",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssignmentId",
                table: "EmployeeAssignments",
                nullable: true);

            migrationBuilder.DropColumn("Date", "Assignments");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Assignments",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAssignments_Assignments_AssignmentId",
                table: "EmployeeAssignments",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "AssignmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAssignments_Users_UserId",
                table: "EmployeeAssignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAssignments_Assignments_AssignmentId",
                table: "EmployeeAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAssignments_Users_UserId",
                table: "EmployeeAssignments");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "EmployeeAssignments",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "AssignmentId",
                table: "EmployeeAssignments",
                nullable: false);

            migrationBuilder.AlterColumn<long>(
                name: "Date",
                table: "Assignments",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAssignments_Assignments_AssignmentId",
                table: "EmployeeAssignments",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "AssignmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAssignments_Users_UserId",
                table: "EmployeeAssignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}