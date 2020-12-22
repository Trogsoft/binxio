using Microsoft.EntityFrameworkCore.Migrations;

namespace Binxio.Data.Migrations
{
    public partial class Log2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescribesContextOperation",
                table: "Log");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Log",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Log");

            migrationBuilder.AddColumn<bool>(
                name: "DescribesContextOperation",
                table: "Log",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
