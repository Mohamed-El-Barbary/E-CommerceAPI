using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSubTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductSubTypeId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductSubType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    productTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSubType_ProductTypes_productTypeId",
                        column: x => x.productTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductSubTypeId",
                table: "Products",
                column: "ProductSubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubType_productTypeId",
                table: "ProductSubType",
                column: "productTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductSubType_ProductSubTypeId",
                table: "Products",
                column: "ProductSubTypeId",
                principalTable: "ProductSubType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductSubType_ProductSubTypeId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductSubType");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductSubTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductSubTypeId",
                table: "Products");
        }
    }
}
