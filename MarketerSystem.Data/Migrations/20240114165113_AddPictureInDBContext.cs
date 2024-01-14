using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketerSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPictureInDBContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Picture_Distributors_DistributorID",
                table: "Picture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Picture",
                table: "Picture");

            migrationBuilder.RenameTable(
                name: "Picture",
                newName: "Pictures");

            migrationBuilder.RenameIndex(
                name: "IX_Picture_DistributorID",
                table: "Pictures",
                newName: "IX_Pictures_DistributorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures",
                column: "ID");

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 1,
                column: "DistributorGuid",
                value: new Guid("c5468775-2de3-41b8-8a97-bbe37fda3209"));

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 2,
                column: "DistributorGuid",
                value: new Guid("fc487ea2-9bc8-4ca3-8283-f05f28dd7980"));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 14, 20, 51, 12, 785, DateTimeKind.Unspecified).AddTicks(7677), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 14, 20, 51, 12, 785, DateTimeKind.Unspecified).AddTicks(7686), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Distributors_DistributorID",
                table: "Pictures",
                column: "DistributorID",
                principalTable: "Distributors",
                principalColumn: "DistributorID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Distributors_DistributorID",
                table: "Pictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures");

            migrationBuilder.RenameTable(
                name: "Pictures",
                newName: "Picture");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_DistributorID",
                table: "Picture",
                newName: "IX_Picture_DistributorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Picture",
                table: "Picture",
                column: "ID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Picture_Distributors_DistributorID",
                table: "Picture",
                column: "DistributorID",
                principalTable: "Distributors",
                principalColumn: "DistributorID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
