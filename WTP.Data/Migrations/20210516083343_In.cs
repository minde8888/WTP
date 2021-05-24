using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.Data.Migrations
{
    public partial class In : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Manager_Id",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Manager",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MobileNumber",
                table: "Manager",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Manager",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Occupation",
                table: "Manager",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Manager",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MobileNumber",
                table: "Employee",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "Occupation",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Manager");

            migrationBuilder.AlterColumn<int>(
                name: "MobileNumber",
                table: "Employee",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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
