using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.Data.Migrations
{
    public partial class add_ManagerId_EmployeeID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Identity",
                table: "Manager",
                newName: "ManagerId");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Identity",
                table: "Employee",
                newName: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManagerId",
                schema: "Identity",
                table: "Manager",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                schema: "Identity",
                table: "Employee",
                newName: "Id");
        }
    }
}
