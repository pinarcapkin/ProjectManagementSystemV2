using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemV2.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminSeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 28, 12, 24, 14, 120, DateTimeKind.Utc).AddTicks(4250));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 28, 12, 24, 14, 120, DateTimeKind.Utc).AddTicks(4250));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 28, 12, 24, 14, 120, DateTimeKind.Utc).AddTicks(4250));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Department", "Email", "FirstName", "IsActive", "IsDeleted", "LastName", "PasswordHash" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "admin@sistem.com", "Admin", true, false, "Kullanici", "$2a$11$fwFerqN2eoLB8Bek3k6OsuMfHdr9jrQMwZah6a4Xh/FrvGai8aNhm" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "AssignedAt", "AssignedByUserId", "IsActive", "RoleId", "UserId" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, 1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

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
    }
}
