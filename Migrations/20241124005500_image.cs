using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookShop.Migrations
{
    /// <inheritdoc />
    public partial class image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("86ca5066-7e71-408b-8d11-d04439a4564f"));

            migrationBuilder.AddColumn<string>(
                name: "ProductImage",
                table: "Book",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserCode", "UserName" },
                values: new object[] { new Guid("6ad96b5b-155c-4a0e-8d69-d20aac67aa91"), "admin@gmail.com", "admin@1234", "Admin", "U0001", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("6ad96b5b-155c-4a0e-8d69-d20aac67aa91"));

            migrationBuilder.DropColumn(
                name: "ProductImage",
                table: "Book");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserCode", "UserName" },
                values: new object[] { new Guid("86ca5066-7e71-408b-8d11-d04439a4564f"), "admin@gmail.com", "admin@1234", "Admin", "U001", "admin" });
        }
    }
}
