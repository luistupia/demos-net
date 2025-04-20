using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShoppingCart.Api.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shopping_carts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    code = table.Column<string>(type: "TEXT", nullable: false),
                    userId = table.Column<string>(type: "TEXT", nullable: false),
                    createOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    createBy = table.Column<string>(type: "TEXT", nullable: false),
                    lastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    lastModifiedBy = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_shopping_carts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    code = table.Column<string>(type: "TEXT", nullable: false),
                    imageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    price = table.Column<decimal>(type: "TEXT", nullable: false),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    cartId = table.Column<Guid>(type: "TEXT", nullable: false),
                    createOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    createBy = table.Column<string>(type: "TEXT", nullable: false),
                    lastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    lastModifiedBy = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_items", x => x.id);
                    table.ForeignKey(
                        name: "fK_items_carts_cartId",
                        column: x => x.cartId,
                        principalTable: "shopping_carts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "shopping_carts",
                columns: new[] { "id", "code", "createBy", "createOn", "lastModifiedBy", "lastModifiedOn", "userId" },
                values: new object[,]
                {
                    { new Guid("514184ba-8650-4f38-a834-78ed115deba4"), "INIT_CODE", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SYSTEM" },
                    { new Guid("c5a99654-a34f-44cd-a1d5-095b2a054f57"), "INIT_PASS", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SYSTEM" }
                });

            migrationBuilder.InsertData(
                table: "items",
                columns: new[] { "id", "cartId", "code", "createBy", "createOn", "description", "imageUrl", "lastModifiedBy", "lastModifiedOn", "name", "price", "quantity" },
                values: new object[,]
                {
                    { new Guid("24c5cf13-3533-4ba9-a32a-5ea8201124e1"), new Guid("514184ba-8650-4f38-a834-78ed115deba4"), "ITEM_CODE", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MODERNO", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PHONE X", 15.00m, 10 },
                    { new Guid("55b1364a-f41e-4c15-aaee-725454d6a38e"), new Guid("514184ba-8650-4f38-a834-78ed115deba4"), "ITEM_AM", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "XXXXXX", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MACBOOK", 13.00m, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "iX_items_cartId",
                table: "items",
                column: "cartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "shopping_carts");
        }
    }
}
