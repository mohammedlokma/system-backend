using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace system_backend.Migrations
{
    public partial class seeddata2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Safe",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Safe",
                columns: new[] { "Id", "Total" },
                values: new object[] { 6, 0.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Safe",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.InsertData(
                table: "Safe",
                columns: new[] { "Id", "Total" },
                values: new object[] { 1, 0.0 });
        }
    }
}
