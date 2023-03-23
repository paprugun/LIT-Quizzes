using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorApp.DAL.Migrations
{
    public partial class Alter_Quiz_TimeToPass_double : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TimeToPass",
                table: "Quizzes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeToPass",
                table: "Quizzes");
        }
    }
}
