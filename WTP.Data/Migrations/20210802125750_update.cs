using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.Data.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_AspNetUsers_ApplicationUserId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Manager_AspNetUsers_ApplicationUserId",
                table: "Manager");

            migrationBuilder.DropIndex(
                name: "IX_Manager_ApplicationUserId",
                table: "Manager");

            migrationBuilder.DropIndex(
                name: "IX_Employee_ApplicationUserId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Employee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Manager",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manager_ApplicationUserId",
                table: "Manager",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ApplicationUserId",
                table: "Employee",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_AspNetUsers_ApplicationUserId",
                table: "Employee",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Manager_AspNetUsers_ApplicationUserId",
                table: "Manager",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
