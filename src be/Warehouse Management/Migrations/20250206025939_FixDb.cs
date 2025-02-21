using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Warehouse_Management.Migrations
{
    /// <inheritdoc />
    public partial class FixDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "373e112e - 121b - 4f3f - bb3e - dc30c08b9999");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9116bb38-85b1-4345-bf4a-7c0819a8ef3b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a589cd2-2113-4106-901d-aba2518cc5b0", null, "Admin", "ADMIN" },
                    { "73373929-3552-4b5f-97c5-4f55c0ce0d0b", null, "Staff", "STAFF" },
                    { "de3be95b-1b82-4803-874a-0cf0e90dabab", null, "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a589cd2-2113-4106-901d-aba2518cc5b0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73373929-3552-4b5f-97c5-4f55c0ce0d0b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de3be95b-1b82-4803-874a-0cf0e90dabab");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "373e112e - 121b - 4f3f - bb3e - dc30c08b9999", "373e112e - 121b - 4f3f - bb3e - dc30c08b9999", "Admin", "ADMIN" },
                    { "9116bb38-85b1-4345-bf4a-7c0819a8ef3b", "9116bb38-85b1-4345-bf4a-7c0819a8ef3b", "Staff", "STAFF" }
                });
        }
    }
}
