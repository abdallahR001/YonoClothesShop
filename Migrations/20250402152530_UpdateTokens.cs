using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YonoClothesShop.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expiration",
                table: "Tokens",
                newName: "RefreshTokenExpiration");

            migrationBuilder.AddColumn<DateTime>(
                name: "AccessTokenExpiration",
                table: "Tokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Tokens",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessTokenExpiration",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Tokens");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiration",
                table: "Tokens",
                newName: "Expiration");
        }
    }
}
