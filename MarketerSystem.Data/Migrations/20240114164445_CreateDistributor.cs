using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MarketerSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDistributor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Distributors",
                columns: table => new
                {
                    DistributorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistributorGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    PassportID = table.Column<int>(type: "int", nullable: false),
                    GenerationLinker = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecomendatorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributors", x => x.DistributorID);
                    table.ForeignKey(
                        name: "FK_Distributors_Distributors_RecomendatorID",
                        column: x => x.RecomendatorID,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID");
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
                name: "Addresses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressType = table.Column<int>(type: "int", nullable: false),
                    AddressInfo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DistributorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Addresses_Distributors_DistributorID",
                        column: x => x.DistributorID,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "ContactInfo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactInformationType = table.Column<int>(type: "int", nullable: false),
                    Information = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DistributorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ContactInfo_Distributors_DistributorID",
                        column: x => x.DistributorID,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passports",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentType = table.Column<int>(type: "int", nullable: false),
                    DocumentSerie = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ReleaseDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExpirationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PersonalNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssuingAgency = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DistributorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Passports_Distributors_DistributorID",
                        column: x => x.DistributorID,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.InsertData(
                table: "Distributors",
                columns: new[] { "DistributorID", "BirthDate", "DistributorGuid", "FirstName", "Gender", "GenerationLinker", "LastName", "PassportID", "RecomendatorID" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5448182b-8909-4ad2-9be0-1e5f2097aed5"), "Zura", 1, null, "Samkharadze", 0, null },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("2a65fe94-3e4d-4f3c-95ee-eb1998783ef6"), "Maiko", 2, null, "Samkharadze", 0, null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ID", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Pen", 10m },
                    { 2, "Pencil", 9m },
                    { 3, "Book", 20m },
                    { 4, "Notepad", 14m }
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "ID", "AddressInfo", "AddressType", "DistributorID" },
                values: new object[,]
                {
                    { 1, "საჯაიას 10", 1, 1 },
                    { 2, "ბოხუას 10", 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "ContactInfo",
                columns: new[] { "ID", "ContactInformationType", "DistributorID", "Information" },
                values: new object[,]
                {
                    { 1, 2, 1, "599473377" },
                    { 2, 3, 2, "MaikoMaiko@Gmail.com" }
                });

            migrationBuilder.InsertData(
                table: "Passports",
                columns: new[] { "ID", "DistributorID", "DocumentNumber", "DocumentSerie", "DocumentType", "ExpirationDate", "IssuingAgency", "PersonalNumber", "ReleaseDate" },
                values: new object[,]
                {
                    { 1, 1, "102340", "11111", 1, new DateTimeOffset(new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), "SA Agency", "01008048552", new DateTimeOffset(new DateTime(2024, 1, 14, 20, 44, 44, 875, DateTimeKind.Unspecified).AddTicks(9418), new TimeSpan(0, 4, 0, 0, 0)) },
                    { 2, 2, "102340", "11111", 1, new DateTimeOffset(new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), "SA Agency", "599473377", new DateTimeOffset(new DateTime(2024, 1, 14, 20, 44, 44, 875, DateTimeKind.Unspecified).AddTicks(9430), new TimeSpan(0, 4, 0, 0, 0)) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_DistributorID",
                table: "Addresses",
                column: "DistributorID");

            migrationBuilder.CreateIndex(
                name: "IX_BonusPayments_DistributorID",
                table: "BonusPayments",
                column: "DistributorID");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_DistributorID",
                table: "ContactInfo",
                column: "DistributorID");

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_DistributorGuid",
                table: "Distributors",
                column: "DistributorGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_RecomendatorID",
                table: "Distributors",
                column: "RecomendatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Passports_DistributorID",
                table: "Passports",
                column: "DistributorID",
                unique: true);

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
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "BonusPayments");

            migrationBuilder.DropTable(
                name: "ContactInfo");

            migrationBuilder.DropTable(
                name: "Passports");

            migrationBuilder.DropTable(
                name: "Sells");

            migrationBuilder.DropTable(
                name: "Distributors");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
