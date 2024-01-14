using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketerSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPictures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Picture",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UploadTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DistributorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picture", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Picture_Distributors_DistributorID",
                        column: x => x.DistributorID,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 1,
                column: "DistributorGuid",
                value: new Guid("20be1849-eb96-4d11-8016-a4da2f8658cc"));

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 2,
                column: "DistributorGuid",
                value: new Guid("0f794ec4-612d-4311-98b4-5b3d82cff52a"));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 14, 20, 47, 6, 29, DateTimeKind.Unspecified).AddTicks(6750), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 14, 20, 47, 6, 29, DateTimeKind.Unspecified).AddTicks(6762), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_Picture_DistributorID",
                table: "Picture",
                column: "DistributorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Picture");

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 1,
                column: "DistributorGuid",
                value: new Guid("5448182b-8909-4ad2-9be0-1e5f2097aed5"));

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 2,
                column: "DistributorGuid",
                value: new Guid("2a65fe94-3e4d-4f3c-95ee-eb1998783ef6"));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 14, 20, 44, 44, 875, DateTimeKind.Unspecified).AddTicks(9418), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 14, 20, 44, 44, 875, DateTimeKind.Unspecified).AddTicks(9430), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
