using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationApp.DataAccessLayer.Migrations
{
    public partial class SoftDeleteForUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorInPrintingEditionEntity");

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "2e1af3c0-e851-4fc6-b503-24b31808fc76");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "dded3cf9-df23-46fa-bbb0-c9cee5a086cd", "AQAAAAEAACcQAAAAEPKZCSCJze1mDUptRKVZliHhySwIe2J72/RqZSStq/hm5gomxdyq9BD1ljcGRHfukg==" });

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 11, 12, 4, 1, 112, DateTimeKind.Local).AddTicks(2109));

            migrationBuilder.UpdateData(
                table: "PrintingEditions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 11, 12, 4, 1, 110, DateTimeKind.Local).AddTicks(4858));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AspNetUsers");

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
    }
}
