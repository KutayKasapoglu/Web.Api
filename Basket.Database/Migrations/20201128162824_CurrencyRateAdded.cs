using Microsoft.EntityFrameworkCore.Migrations;

namespace Basket.Database.Migrations
{
    public partial class CurrencyRateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrencyRate",
                table: "BasketProducts",
                type: "decimal(12,4)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyRate",
                table: "BasketProducts");
        }
    }
}
