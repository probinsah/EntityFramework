using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DbOperationsWithEFCoreApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedColumnData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyTypeId",
                table: "BookPrice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "CurrencyTypes",
                columns: new[] { "Id", "Currency", "Description" },
                values: new object[,]
                {
                    { 1, "INR", "Indian Rupee" },
                    { 2, "USD", "US Dollar" },
                    { 3, "TL", "Turkish Lira" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookPrice_BookId",
                table: "BookPrice",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookPrice_CurrencyTypeId",
                table: "BookPrice",
                column: "CurrencyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookPrice_Books_BookId",
                table: "BookPrice",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookPrice_CurrencyTypes_CurrencyTypeId",
                table: "BookPrice",
                column: "CurrencyTypeId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookPrice_Books_BookId",
                table: "BookPrice");

            migrationBuilder.DropForeignKey(
                name: "FK_BookPrice_CurrencyTypes_CurrencyTypeId",
                table: "BookPrice");

            migrationBuilder.DropIndex(
                name: "IX_BookPrice_BookId",
                table: "BookPrice");

            migrationBuilder.DropIndex(
                name: "IX_BookPrice_CurrencyTypeId",
                table: "BookPrice");

            migrationBuilder.DeleteData(
                table: "CurrencyTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CurrencyTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CurrencyTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "CurrencyTypeId",
                table: "BookPrice");
        }
    }
}
