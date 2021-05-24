using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Manager_Id",
                table: "Employee");

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerRef",
                table: "Employee",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ManagerRef",
                table: "Employee",
                column: "ManagerRef",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Manager_ManagerRef",
                table: "Employee",
                column: "ManagerRef",
                principalTable: "Manager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Manager_ManagerRef",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ManagerRef",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ManagerRef",
                table: "Employee");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Manager_Id",
                table: "Employee",
                column: "Id",
                principalTable: "Manager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
