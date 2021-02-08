using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationApp.DataAccessLayer.Migrations
{
    public partial class InitialBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PrintingEditions",
                columns: new[] { "Id", "CreatedAt", "Currency", "Description", "IsRemoved", "Price", "Status", "Title", "Type" },
                values: new object[] { 1, new DateTime(2021, 2, 8, 17, 34, 21, 598, DateTimeKind.Local).AddTicks(4689), 5, "Initial test PE", false, 200.05m, 0, "Test PE", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PrintingEditions",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
