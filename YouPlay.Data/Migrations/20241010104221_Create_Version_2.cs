using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YouPlay.Data.Migrations
{
    /// <inheritdoc />
    public partial class Create_Version_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PurchaseItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PurchaseItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PurchaseItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "PurchaseItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PurchaseItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PurchaseItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PurchaseItems");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "PurchaseItems");
        }
    }
}
