using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace system_backend.Migrations
{
    public partial class editagentmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Agents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Agents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Agents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Agents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Agents",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Agents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Agents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Agents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
