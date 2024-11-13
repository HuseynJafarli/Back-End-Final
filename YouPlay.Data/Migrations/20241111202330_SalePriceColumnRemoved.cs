using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YouPlay.Data.Migrations
{
    /// <inheritdoc />
    public partial class SalePriceColumnRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "Games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SalePrice",
                table: "Games",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
