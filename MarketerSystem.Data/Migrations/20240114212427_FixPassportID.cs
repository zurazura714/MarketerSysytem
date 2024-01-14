using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketerSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPassportID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 1,
                columns: new[] { "DistributorGuid", "PassportID" },
                values: new object[] { new Guid("2a6a44b7-dabd-41ea-94e3-8ed42320100f"), 1 });

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 2,
                columns: new[] { "DistributorGuid", "PassportID" },
                values: new object[] { new Guid("d222a60b-978f-48a5-b0dd-3301da9783fa"), 2 });

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 1, 24, 27, 110, DateTimeKind.Unspecified).AddTicks(9408), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 1, 24, 27, 110, DateTimeKind.Unspecified).AddTicks(9421), new TimeSpan(0, 4, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 1,
                columns: new[] { "DistributorGuid", "PassportID" },
                values: new object[] { new Guid("ceab9a66-833d-4df8-91c9-4f70d7e66cfb"), 0 });

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 2,
                columns: new[] { "DistributorGuid", "PassportID" },
                values: new object[] { new Guid("e75dbdfd-558a-4fa4-9db7-d1af2c8f0bec"), 0 });

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 1,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 0, 49, 46, 546, DateTimeKind.Unspecified).AddTicks(9710), new TimeSpan(0, 4, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Passports",
                keyColumn: "ID",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTimeOffset(new DateTime(2024, 1, 15, 0, 49, 46, 546, DateTimeKind.Unspecified).AddTicks(9723), new TimeSpan(0, 4, 0, 0, 0)));
        }
    }
}
