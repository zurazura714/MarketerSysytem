using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketerSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrectSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 1,
                column: "DistributorGuid",
                value: new Guid("ceab9a66-833d-4df8-91c9-4f70d7e66cfb"));

            migrationBuilder.UpdateData(
                table: "Distributors",
                keyColumn: "DistributorID",
                keyValue: 2,
                column: "DistributorGuid",
                value: new Guid("e75dbdfd-558a-4fa4-9db7-d1af2c8f0bec"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
