using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationApp.DataAccessLayer.Migrations
{
    public partial class LogTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorInPrintingEditionEntity",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    PrintingEditionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorInPrintingEditionEntity", x => new { x.AuthorsId, x.PrintingEditionsId });
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "6b94bf55-687d-462c-a254-ce209a41c84e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b1c46692-ec2f-4ee9-b831-abbcdcd534b1", "AQAAAAEAACcQAAAAEPG9fXV2DrNZfVjIjCzi1ZCs4ei819MRg3KGCoqQAVo2fpCaGlzDoFvKFp9FEeWpZw==" });

            migrationBuilder.InsertData(
                table: "AuthorInPrintingEditionEntity",
                columns: new[] { "AuthorsId", "PrintingEditionsId" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 9, 11, 26, 37, 408, DateTimeKind.Local).AddTicks(5284));

            migrationBuilder.UpdateData(
                table: "PrintingEditions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 9, 11, 26, 37, 406, DateTimeKind.Local).AddTicks(7903));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorInPrintingEditionEntity");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "707464c9-91ac-4318-a371-3f918b31eae2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "29406ee5-f07c-4d83-bb35-bb4bb707d83c", "AQAAAAEAACcQAAAAEDWsEimHJ3jXonh4E3a35NASqT1vB7gT11RpsZOTDUuUDunp5CcUoSXtKRxBNEoisQ==" });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 8, 18, 1, 1, 63, DateTimeKind.Local).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "PrintingEditions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 8, 18, 1, 1, 59, DateTimeKind.Local).AddTicks(3994));
        }
    }
}
