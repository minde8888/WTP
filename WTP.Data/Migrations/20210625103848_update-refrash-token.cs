using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.Data.Migrations
{
    public partial class updaterefrashtoken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRevorked",
                table: "RefreshToken",
                newName: "IsRevoked");

            migrationBuilder.RenameColumn(
                name: "ExpirysDate",
                table: "RefreshToken",
                newName: "ExpiryDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRevoked",
                table: "RefreshToken",
                newName: "IsRevorked");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "RefreshToken",
                newName: "ExpirysDate");
        }
    }
}
