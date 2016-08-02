using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AWA.Migrations
{
    public partial class SaveDateTimeAsNullableLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Date", "Assignments");

            migrationBuilder.AddColumn<long>(
                name: "Date",
                table: "Assignments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Assignments",
                nullable: false);
        }
    }
}
