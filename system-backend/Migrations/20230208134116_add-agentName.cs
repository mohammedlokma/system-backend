using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace system_backend.Migrations
{
    public partial class addagentName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgentName",
                table: "FullReport",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgentName",
                table: "FullReport");
        }
    }
}
