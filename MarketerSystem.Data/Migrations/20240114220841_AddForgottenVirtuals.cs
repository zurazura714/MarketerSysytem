using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketerSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddForgottenVirtuals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 1,
                column: "DistributorGuid",
                value: new Guid("8d03f809-17ea-433e-8408-2e6d9823f9d5"));

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 2,
                column: "DistributorGuid",
                value: new Guid("9c18bb33-f73f-4f9f-aced-892179b04a83"));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 2, 8, 41, 390, DateTimeKind.Unspecified).AddTicks(4200), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 2, 8, 41, 390, DateTimeKind.Unspecified).AddTicks(4212), new TimeSpan(0, 4, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 1,
                column: "DistributorGuid",
                value: new Guid("0c3b5913-f41f-4e3b-96f0-b62a194c2781"));

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 2,
                column: "DistributorGuid",
                value: new Guid("1cba4aa7-2b5b-4894-8fd7-7eaf77760138"));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 2, 2, 24, 990, DateTimeKind.Unspecified).AddTicks(4600), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 2, 2, 24, 990, DateTimeKind.Unspecified).AddTicks(4613), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
