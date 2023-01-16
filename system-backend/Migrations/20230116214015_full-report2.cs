using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace system_backend.Migrations
{
    public partial class fullreport2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyReportItems_ReportItems_ReportItemsId",
                table: "CompanyReportItems");

            migrationBuilder.DropIndex(
                name: "IX_CompanyReportItems_ReportItemsId",
                table: "CompanyReportItems");

            migrationBuilder.DropColumn(
                name: "ReportItemsId",
                table: "CompanyReportItems");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyReportItems_ReportItemId",
                table: "CompanyReportItems",
                column: "ReportItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyReportItems_ReportItems_ReportItemId",
                table: "CompanyReportItems",
                column: "ReportItemId",
                principalTable: "ReportItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyReportItems_ReportItems_ReportItemId",
                table: "CompanyReportItems");

            migrationBuilder.DropIndex(
                name: "IX_CompanyReportItems_ReportItemId",
                table: "CompanyReportItems");

            migrationBuilder.AddColumn<int>(
                name: "ReportItemsId",
                table: "CompanyReportItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyReportItems_ReportItemsId",
                table: "CompanyReportItems",
                column: "ReportItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyReportItems_ReportItems_ReportItemsId",
                table: "CompanyReportItems",
                column: "ReportItemsId",
                principalTable: "ReportItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
