using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.Data.Migrations
{
    public partial class Update1RenamedIdentityTableNames : Migration
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

            migrationBuilder.DropColumn(
                name: "EmployeesId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                schema: "Identity",
                table: "AspNetUsers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_ManagerId",
                schema: "Identity",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_UserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employee_UserId",
                schema: "Identity",
                table: "AspNetUsers",
                column: "UserId",
                principalSchema: "Identity",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Manager_UserId",
                schema: "Identity",
                table: "AspNetUsers",
                column: "UserId",
                principalSchema: "Identity",
                principalTable: "Manager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employee_UserId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Manager_UserId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Identity",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Identity",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "Identity",
                table: "AspNetUsers",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_UserId",
                schema: "Identity",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_ManagerId");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeesId",
                schema: "Identity",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeesId",
                schema: "Identity",
                table: "AspNetUsers",
                column: "EmployeesId");

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
