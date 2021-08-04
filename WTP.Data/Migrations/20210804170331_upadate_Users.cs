using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.Data.Migrations
{
    public partial class upadate_Users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employee_EmployeesId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Manager_ManagerId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeesId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ManagerId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeesId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "Identity",
                table: "Manager",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "Identity",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manager_UserId",
                schema: "Identity",
                table: "Manager",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UserId",
                schema: "Identity",
                table: "Employee",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_AspNetUsers_UserId",
                schema: "Identity",
                table: "Employee",
                column: "UserId",
                principalSchema: "Identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_AspNetUsers_UserId",
                schema: "Identity",
                table: "Manager",
                column: "UserId",
                principalSchema: "Identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_AspNetUsers_UserId",
                schema: "Identity",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Manager_AspNetUsers_UserId",
                schema: "Identity",
                table: "Manager");

            migrationBuilder.DropIndex(
                name: "IX_Manager_UserId",
                schema: "Identity",
                table: "Manager");

            migrationBuilder.DropIndex(
                name: "IX_Employee_UserId",
                schema: "Identity",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Identity",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Identity",
                table: "Employee");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeesId",
                schema: "Identity",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                schema: "Identity",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "Identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeesId",
                schema: "Identity",
                table: "AspNetUsers",
                column: "EmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ManagerId",
                schema: "Identity",
                table: "AspNetUsers",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employee_EmployeesId",
                schema: "Identity",
                table: "AspNetUsers",
                column: "EmployeesId",
                principalSchema: "Identity",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Manager_ManagerId",
                schema: "Identity",
                table: "AspNetUsers",
                column: "ManagerId",
                principalSchema: "Identity",
                principalTable: "Manager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
