using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorApp.DAL.Migrations
{
    public partial class Alter_UsersResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAt",
                table: "UsersResults",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "UsersResults");
        }
    }
}
