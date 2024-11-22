using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookShop.Migrations
{
    /// <inheritdoc />
    public partial class updateuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("e063510c-b095-46ce-b61b-003063405193"));

            migrationBuilder.AddColumn<string>(
                name: "UserCode",
                table: "users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserCode", "UserName" },
                values: new object[] { new Guid("9c9a115f-6c27-44b4-b9b0-7c095767f76a"), "admin@gmail.com", "admin@1234", "Admin", "U001", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("9c9a115f-6c27-44b4-b9b0-7c095767f76a"));

            migrationBuilder.DropColumn(
                name: "UserCode",
                table: "users");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserName" },
                values: new object[] { new Guid("e063510c-b095-46ce-b61b-003063405193"), "admin@gmail.com", "admin@1234", "Admin", "admin" });
        }
    }
}
