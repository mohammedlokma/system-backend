using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace system_backend.Migrations
{
    public partial class addagentserviceplaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgentServicePlaces_Agents_AgentsId",
                table: "AgentServicePlaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgentServicePlaces",
                table: "AgentServicePlaces");

            migrationBuilder.RenameColumn(
                name: "AgentsId",
                table: "AgentServicePlaces",
                newName: "AgentId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AgentServicePlaces",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "AgentServicePlaces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServicePlacesId",
                table: "Agents",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgentServicePlaces",
                table: "AgentServicePlaces",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AgentServicePlaces_AgentId",
                table: "AgentServicePlaces",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Agents_ServicePlacesId",
                table: "Agents",
                column: "ServicePlacesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agents_ServicePlaces_ServicePlacesId",
                table: "Agents",
                column: "ServicePlacesId",
                principalTable: "ServicePlaces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AgentServicePlaces_Agents_AgentId",
                table: "AgentServicePlaces",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agents_ServicePlaces_ServicePlacesId",
                table: "Agents");

            migrationBuilder.DropForeignKey(
                name: "FK_AgentServicePlaces_Agents_AgentId",
                table: "AgentServicePlaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgentServicePlaces",
                table: "AgentServicePlaces");

            migrationBuilder.DropIndex(
                name: "IX_AgentServicePlaces_AgentId",
                table: "AgentServicePlaces");

            migrationBuilder.DropIndex(
                name: "IX_Agents_ServicePlacesId",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AgentServicePlaces");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "AgentServicePlaces");

            migrationBuilder.DropColumn(
                name: "ServicePlacesId",
                table: "Agents");

            migrationBuilder.RenameColumn(
                name: "AgentId",
                table: "AgentServicePlaces",
                newName: "AgentsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgentServicePlaces",
                table: "AgentServicePlaces",
                columns: new[] { "AgentsId", "ServicePlacesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AgentServicePlaces_Agents_AgentsId",
                table: "AgentServicePlaces",
                column: "AgentsId",
                principalTable: "Agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
