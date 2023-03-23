using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorApp.DAL.Migrations
{
    public partial class Alter_Quiz_TimeToPass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeToPass",
                table: "Quizzes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeToPass",
                table: "Quizzes",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
