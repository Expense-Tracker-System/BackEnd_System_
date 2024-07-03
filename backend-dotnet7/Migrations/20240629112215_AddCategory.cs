using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend_dotnet7.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Created" },
                values: new object[] { 1, new DateTime(2024, 6, 29, 16, 52, 14, 801, DateTimeKind.Local).AddTicks(5950) });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Eleccity Bill" },
                    { 2, "Water bill" },
                    { 3, "Travel" },
                    { 4, "Medicine" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CategoryId", "Created", "Note", "Status" },
                values: new object[,]
                {
                    { 1, 200, 2, new DateTime(2024, 6, 29, 16, 52, 14, 801, DateTimeKind.Local).AddTicks(5963), "Elecity Bill", 2 },
                    { 2, 500, 3, new DateTime(2024, 6, 29, 16, 52, 14, 801, DateTimeKind.Local).AddTicks(5964), "water Bill", 2 },
                    { 3, 1000, 4, new DateTime(2024, 6, 29, 16, 52, 14, 801, DateTimeKind.Local).AddTicks(5966), "Medicine", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_categories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_categories_CategoryId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions");

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Transactions");

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4,
                column: "Created",
                value: new DateTime(2024, 6, 29, 11, 31, 47, 694, DateTimeKind.Local).AddTicks(3339));
        }
    }
}
