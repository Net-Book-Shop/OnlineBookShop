using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineBookShop.Migrations
{
    /// <inheritdoc />
    public partial class nochange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("9b8eff43-0cc4-48d9-a21c-862aed85d99b"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserCode", "UserName" },
                values: new object[] { new Guid("31b9b91f-5173-498e-8776-284a02edb873"), "admin@gmail.com", "AQAAAAIAAYagAAAAELq2qR45s2GfiVsQnKKHUotwlOVxItwFpuar8SKIIrlxaEBIxGtx0yzeipsx0EeLzg==", "Admin", "U0001", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("31b9b91f-5173-498e-8776-284a02edb873"));

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Password", "Role", "UserCode", "UserName" },
                values: new object[] { new Guid("9b8eff43-0cc4-48d9-a21c-862aed85d99b"), "admin@gmail.com", "admin@1234", "Admin", "U0001", "admin" });
        }
    }
}
