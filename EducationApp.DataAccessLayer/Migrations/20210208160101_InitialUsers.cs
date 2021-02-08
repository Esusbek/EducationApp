using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationApp.DataAccessLayer.Migrations
{
    public partial class InitialUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1", "707464c9-91ac-4318-a371-3f918b31eae2", "Admin", "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "29406ee5-f07c-4d83-bb35-bb4bb707d83c", "test@test.com", true, "Vitaly", "Pupkin", false, null, "test@test.com", "admin", "AQAAAAEAACcQAAAAEDWsEimHJ3jXonh4E3a35NASqT1vB7gT11RpsZOTDUuUDunp5CcUoSXtKRxBNEoisQ==", null, false, "", false, "Admin" });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "CreatedAt", "IsRemoved", "Name" },
                values: new object[] { 1, new DateTime(2021, 2, 8, 18, 1, 1, 63, DateTimeKind.Local).AddTicks(3273), false, "Vasily" });

            migrationBuilder.UpdateData(
                table: "PrintingEditions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 8, 18, 1, 1, 59, DateTimeKind.Local).AddTicks(3994));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "1" });

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.UpdateData(
                table: "PrintingEditions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 8, 17, 34, 21, 598, DateTimeKind.Local).AddTicks(4689));
        }
    }
}
