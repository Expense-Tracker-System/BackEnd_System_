using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_dotnet7.Migrations
{
    /// <inheritdoc />
    public partial class px : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "SavingViewEntitiess",
                newName: "userName");

            migrationBuilder.RenameColumn(
                name: "Month",
                table: "SavingViewEntitiess",
                newName: "BankName");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "TransactionEntities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "SavingViewEntitiess",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "SavingViewEntitiess",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankName",
                table: "TransactionEntities");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "SavingViewEntitiess");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "SavingViewEntitiess");

            migrationBuilder.RenameColumn(
                name: "userName",
                table: "SavingViewEntitiess",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "BankName",
                table: "SavingViewEntitiess",
                newName: "Month");
        }
    }
}
