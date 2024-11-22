using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookShop.Migrations
{
    /// <inheritdoc />
    public partial class booktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("9c9a115f-6c27-44b4-b9b0-7c095767f76a"));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Book",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "Book",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserCode", "UserName" },
                values: new object[] { new Guid("86ca5066-7e71-408b-8d11-d04439a4564f"), "admin@gmail.com", "admin@1234", "Admin", "U001", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("86ca5066-7e71-408b-8d11-d04439a4564f"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "Book");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserCode", "UserName" },
                values: new object[] { new Guid("9c9a115f-6c27-44b4-b9b0-7c095767f76a"), "admin@gmail.com", "admin@1234", "Admin", "U001", "admin" });
        }
    }
}
