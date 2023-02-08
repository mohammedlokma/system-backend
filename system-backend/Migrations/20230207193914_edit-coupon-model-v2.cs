using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace system_backend.Migrations
{
    public partial class editcouponmodelv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Companies_CompnyId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_CompnyId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "CompnyId",
                table: "Coupons");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Coupons");

            migrationBuilder.AddColumn<string>(
                name: "CompnyId",
                table: "Coupons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_CompnyId",
                table: "Coupons",
                column: "CompnyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Companies_CompnyId",
                table: "Coupons",
                column: "CompnyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
