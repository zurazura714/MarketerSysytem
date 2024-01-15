using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketerSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class addSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BonusPayments",
                columns: new[] { "ID", "BonusPay", "DistributorID", "FromDate", "ToDate" },
                values: new object[] { 1, 10m, 1, new DateTimeOffset(new DateTime(2024, 1, 15, 8, 46, 56, 724, DateTimeKind.Unspecified).AddTicks(7196), new TimeSpan(0, 4, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 1, 15, 13, 46, 56, 724, DateTimeKind.Unspecified).AddTicks(7205), new TimeSpan(0, 4, 0, 0, 0)) });

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
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 13, 46, 56, 724, DateTimeKind.Unspecified).AddTicks(7103), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 13, 46, 56, 724, DateTimeKind.Unspecified).AddTicks(7118), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "Sells",
                columns: new[] { "ID", "DistributorID", "ProductID", "ProductPrice", "ProductTotalPrice", "ProductUnitPrice", "SoldDate", "UsedForPayment" },
                values: new object[] { 1, 1, 1, 10m, 10m, 10m, new DateTimeOffset(new DateTime(2024, 1, 15, 13, 46, 56, 724, DateTimeKind.Unspecified).AddTicks(7183), new TimeSpan(0, 4, 0, 0, 0)), false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BonusPayments",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sells",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 1,
                column: "DistributorGuid",
                value: new Guid("6aa71073-e08b-4166-97ee-b08ffc0ab96e"));

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 2,
                column: "DistributorGuid",
                value: new Guid("099f8157-6f35-486f-aba3-37577b2e079f"));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 4, 28, 6, 227, DateTimeKind.Unspecified).AddTicks(9111), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 4, 28, 6, 227, DateTimeKind.Unspecified).AddTicks(9123), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
