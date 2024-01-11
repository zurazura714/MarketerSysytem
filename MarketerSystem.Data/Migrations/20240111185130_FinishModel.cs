using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketerSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinishModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BonusPayments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BonusPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FromDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ToDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DistributorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusPayments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BonusPayments_Distributors_DistributorID",
                        column: x => x.DistributorID,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Sells",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistributorID = table.Column<int>(type: "int", nullable: false),
                    SoldDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductTotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sells", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sells_Distributors_DistributorID",
                        column: x => x.DistributorID,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sells_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BonusPayments_DistributorID",
                table: "BonusPayments",
                column: "DistributorID");

            migrationBuilder.CreateIndex(
                name: "IX_Sells_DistributorID",
                table: "Sells",
                column: "DistributorID");

            migrationBuilder.CreateIndex(
                name: "IX_Sells_ProductID",
                table: "Sells",
                column: "ProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BonusPayments");

            migrationBuilder.DropTable(
                name: "Sells");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
