using Microsoft.EntityFrameworkCore.Migrations;

namespace CarsWebApp.Data.Migrations
{
    public partial class fixSearchKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SearchKey",
                table: "Cars",
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SearchKey",
                table: "Cars",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
