using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketerSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDistributor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Passport",
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
                    IssuingAgency = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passport", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    PictureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UploadTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.PictureId);
                });

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
                    PictureId = table.Column<int>(type: "int", nullable: false),
                    PassportID = table.Column<int>(type: "int", nullable: false),
                    GenerationLinker = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Distributors_Passport_PassportID",
                        column: x => x.PassportID,
                        principalTable: "Passport",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Distributors_Picture_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Picture",
                        principalColumn: "PictureId",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_DistributorID",
                table: "Addresses",
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
                name: "IX_Distributors_PassportID",
                table: "Distributors",
                column: "PassportID");

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_PictureId",
                table: "Distributors",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_RecomendatorID",
                table: "Distributors",
                column: "RecomendatorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "ContactInfo");

            migrationBuilder.DropTable(
                name: "Distributors");

            migrationBuilder.DropTable(
                name: "Passport");

            migrationBuilder.DropTable(
                name: "Picture");
        }
    }
}
