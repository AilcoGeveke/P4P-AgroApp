using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AWA.Migrations
{
    public partial class MakeVehiclesOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetRecords_Attachments_AttachmentId",
                table: "TimesheetRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetRecords_Machines_MachineId",
                table: "TimesheetRecords");

            migrationBuilder.AlterColumn<int>(
                name: "MachineId",
                table: "TimesheetRecords",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AttachmentId",
                table: "TimesheetRecords",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetRecords_Attachments_AttachmentId",
                table: "TimesheetRecords",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetRecords_Machines_MachineId",
                table: "TimesheetRecords",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "MachineId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetRecords_Attachments_AttachmentId",
                table: "TimesheetRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_TimesheetRecords_Machines_MachineId",
                table: "TimesheetRecords");

            migrationBuilder.AlterColumn<int>(
                name: "MachineId",
                table: "TimesheetRecords",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "AttachmentId",
                table: "TimesheetRecords",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetRecords_Attachments_AttachmentId",
                table: "TimesheetRecords",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimesheetRecords_Machines_MachineId",
                table: "TimesheetRecords",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "MachineId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
