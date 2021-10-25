using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.Data.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileNumber",
                schema: "Identity",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                schema: "Identity",
                table: "Employee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MobileNumber",
                schema: "Identity",
                table: "Manager",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MobileNumber",
                schema: "Identity",
                table: "Employee",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
