using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemV2.Migrations
{
    /// <inheritdoc />
    public partial class FixUserFieldNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "IsActıve",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Depertment",
                table: "Users",
                newName: "Department");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 11, 17, 44, 634, DateTimeKind.Utc).AddTicks(9090));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 11, 17, 44, 634, DateTimeKind.Utc).AddTicks(9090));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 11, 17, 44, 634, DateTimeKind.Utc).AddTicks(9090));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "IsActıve");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "Users",
                newName: "Depertment");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 9, 44, 59, 82, DateTimeKind.Utc).AddTicks(6910));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 9, 44, 59, 82, DateTimeKind.Utc).AddTicks(6920));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 22, 9, 44, 59, 82, DateTimeKind.Utc).AddTicks(6920));
        }
    }
}
