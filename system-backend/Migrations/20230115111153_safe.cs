using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace system_backend.Migrations
{
    public partial class safe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Safe",
                table: "Safe");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Safe",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Safe",
                table: "Safe",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Safe",
                table: "Safe");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Safe");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Safe",
                table: "Safe",
                column: "Total");
        }
    }
}
