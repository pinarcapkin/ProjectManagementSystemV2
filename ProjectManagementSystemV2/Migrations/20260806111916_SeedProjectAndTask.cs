using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectManagementSystemV2.Migrations
{
    /// <inheritdoc />
    public partial class SeedProjectAndTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "ArchivedAt", "CreatedAt", "Description", "EndDate", "IsActive", "IsArchived", "IsDeleted", "StartDate", "Status", "Title", "UserId" },
                values: new object[] { 9001, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Şirketin kurumsal web sitesinin modern bir tasarımla yenilenmesi", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, false, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Örnek Proje: Web Sitesi Yenileme", 1 });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 6, 11, 19, 15, 829, DateTimeKind.Utc).AddTicks(7930));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 6, 11, 19, 15, 829, DateTimeKind.Utc).AddTicks(7930));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 6, 11, 19, 15, 829, DateTimeKind.Utc).AddTicks(7930));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$tlV9zv6H/hm6hiQ/ZlgMZel65dKHteIwumznxxfu/ZvHTxd9lr7Vi");

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AssignedToUserId", "CompletedAt", "CreatedAt", "CreatedByUserId", "Description", "DueDate", "EstimatedHours", "IsDeleted", "Priority", "ProjectId", "Status", "Title", "UpdatedAt" },
                values: new object[] { 9001, 1, null, new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Yeni ana sayfa mockup'ına göre HTML/CSS kodlanacak", new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, 2, 9001, 0, "Ana sayfa tasarımını tamamla", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 9001);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 9001);

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$fwFerqN2eoLB8Bek3k6OsuMfHdr9jrQMwZah6a4Xh/FrvGai8aNhm");
        }
    }
}
