using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.Data.Migrations
{
    public partial class up : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCreated",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "UserUpdated",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "UserCreated",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UserUpdated",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UserCreated",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "UserUpdated",
                table: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserCreated",
                table: "Manager",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserUpdated",
                table: "Manager",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserCreated",
                table: "Employee",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserUpdated",
                table: "Employee",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserCreated",
                table: "Address",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserUpdated",
                table: "Address",
                type: "uuid",
                nullable: true);
        }
    }
}
