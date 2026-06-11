using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketerSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeterministicSeedAndMoneyPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BonusPayments",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 14, 19, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 1,
                column: "DistributorGuid",
                value: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"));

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 2,
                column: "DistributorGuid",
                value: new Guid("e2c1a7e0-4f2b-4f6a-9b3a-1c9d2e8f4a5b"));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "ExpirationDate", "ReleaseDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "ExpirationDate", "ReleaseDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Sells",
                keyColumn: "ID",
                keyValue: 1,
                column: "SoldDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BonusPayments",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "FromDate", "ToDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 1, 15, 8, 46, 56, 724, DateTimeKind.Unspecified).AddTicks(7196), new TimeSpan(0, 4, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 1, 15, 13, 46, 56, 724, DateTimeKind.Unspecified).AddTicks(7205), new TimeSpan(0, 4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 1,
                column: "DistributorGuid",
                value: new Guid("5a7d104c-7335-4e91-9837-f3d58019781a"));

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 2,
                column: "DistributorGuid",
                value: new Guid("10257356-be2b-4443-a63d-f7b47c39d5c2"));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "ExpirationDate", "ReleaseDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 1, 15, 13, 46, 56, 724, DateTimeKind.Unspecified).AddTicks(7103), new TimeSpan(0, 4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "ExpirationDate", "ReleaseDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 1, 15, 13, 46, 56, 724, DateTimeKind.Unspecified).AddTicks(7118), new TimeSpan(0, 4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Sells",
                keyColumn: "ID",
                keyValue: 1,
                column: "SoldDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 13, 46, 56, 724, DateTimeKind.Unspecified).AddTicks(7183), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
