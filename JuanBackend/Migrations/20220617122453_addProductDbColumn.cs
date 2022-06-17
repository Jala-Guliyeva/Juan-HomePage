using Microsoft.EntityFrameworkCore.Migrations;

namespace JuanBackend.Migrations
{
    public partial class addProductDbColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DisCountPrice",
                table: "Products",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisCountPrice",
                table: "Products");
        }
    }
}
