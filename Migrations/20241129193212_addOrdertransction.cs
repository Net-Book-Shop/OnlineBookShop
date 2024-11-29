using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookShop.Migrations
{
    /// <inheritdoc />
    public partial class addOrdertransction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("6ad96b5b-155c-4a0e-8d69-d20aac67aa91"));

            migrationBuilder.AddColumn<string>(
                name: "BankTransactionId",
                table: "Orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserCode", "UserName" },
                values: new object[] { new Guid("9b8eff43-0cc4-48d9-a21c-862aed85d99b"), "admin@gmail.com", "admin@1234", "Admin", "U0001", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("9b8eff43-0cc4-48d9-a21c-862aed85d99b"));

            migrationBuilder.DropColumn(
                name: "BankTransactionId",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserCode", "UserName" },
                values: new object[] { new Guid("6ad96b5b-155c-4a0e-8d69-d20aac67aa91"), "admin@gmail.com", "admin@1234", "Admin", "U0001", "admin" });
        }
    }
}
