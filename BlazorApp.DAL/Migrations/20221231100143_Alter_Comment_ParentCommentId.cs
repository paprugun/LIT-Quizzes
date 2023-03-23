using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorApp.DAL.Migrations
{
    public partial class Alter_Comment_ParentCommentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentCommentId",
                table: "Comments",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "Comments");
        }
    }
}
