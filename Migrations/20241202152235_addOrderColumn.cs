using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookShop.Migrations
{
    /// <inheritdoc />
    public partial class addOrderColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("31b9b91f-5173-498e-8776-284a02edb873"));

            migrationBuilder.AddColumn<string>(
                name: "CreateUser",
                table: "Orders",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserCode", "UserName" },
                values: new object[] { new Guid("9c630ce5-c3e0-4333-a2fe-6e3b11b465a3"), "admin@gmail.com", "AQAAAAIAAYagAAAAEAhOOieON68J8/WZwDpCcwt0bukXvqFLTpHrmDeE3jMNQFnezWH7z+7rWskJ45ZPkw==", "Admin", "U0001", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("9c630ce5-c3e0-4333-a2fe-6e3b11b465a3"));

            migrationBuilder.DropColumn(
                name: "CreateUser",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserCode", "UserName" },
                values: new object[] { new Guid("31b9b91f-5173-498e-8776-284a02edb873"), "admin@gmail.com", "AQAAAAIAAYagAAAAELq2qR45s2GfiVsQnKKHUotwlOVxItwFpuar8SKIIrlxaEBIxGtx0yzeipsx0EeLzg==", "Admin", "U0001", "admin" });
        }
    }
}
