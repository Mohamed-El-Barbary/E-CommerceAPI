using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterProductSubTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubType_ProductTypes_productTypeId",
                table: "ProductSubType");

            migrationBuilder.RenameColumn(
                name: "productTypeId",
                table: "ProductSubType",
                newName: "ProductTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSubType_productTypeId",
                table: "ProductSubType",
                newName: "IX_ProductSubType_ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubType_ProductTypes_ProductTypeId",
                table: "ProductSubType",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSubType_ProductTypes_ProductTypeId",
                table: "ProductSubType");

            migrationBuilder.RenameColumn(
                name: "ProductTypeId",
                table: "ProductSubType",
                newName: "productTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductSubType_ProductTypeId",
                table: "ProductSubType",
                newName: "IX_ProductSubType_productTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSubType_ProductTypes_productTypeId",
                table: "ProductSubType",
                column: "productTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
