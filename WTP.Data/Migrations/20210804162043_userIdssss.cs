using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WTP.Data.Migrations
{
    public partial class userIdssss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Employee_EmployeeId",
                schema: "Identity",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_Manager_ManagerId",
                schema: "Identity",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employee_EmployeesId1",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Manager_ManagerId1",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Employee_EmployeeId",
                schema: "Identity",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Manager_ManagerId",
                schema: "Identity",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Employee",
                schema: "Identity");

            migrationBuilder.DropIndex(
                name: "IX_Posts_EmployeeId",
                schema: "Identity",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ManagerId",
                schema: "Identity",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeesId1",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Manager",
                schema: "Identity",
                table: "Manager");

            migrationBuilder.DropColumn(
                name: "EmployeesId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeesId1",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Manager",
                schema: "Identity",
                newName: "BaseEntyti",
                newSchema: "Identity");

            migrationBuilder.RenameColumn(
                name: "ManagerId1",
                schema: "Identity",
                table: "AspNetUsers",
                newName: "BaseEntytiId");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                schema: "Identity",
                table: "AspNetUsers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_ManagerId1",
                schema: "Identity",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_BaseEntytiId");

            migrationBuilder.AddColumn<Guid>(
                name: "BaseEntytiId",
                schema: "Identity",
                table: "Posts",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "Identity",
                table: "BaseEntyti",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                schema: "Identity",
                table: "BaseEntyti",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaseEntyti",
                schema: "Identity",
                table: "BaseEntyti",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BaseEntytiId",
                schema: "Identity",
                table: "Posts",
                column: "BaseEntytiId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntyti_ManagerId",
                schema: "Identity",
                table: "BaseEntyti",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_BaseEntyti_EmployeeId",
                schema: "Identity",
                table: "Address",
                column: "EmployeeId",
                principalSchema: "Identity",
                principalTable: "BaseEntyti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_BaseEntyti_ManagerId",
                schema: "Identity",
                table: "Address",
                column: "ManagerId",
                principalSchema: "Identity",
                principalTable: "BaseEntyti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BaseEntyti_BaseEntytiId",
                schema: "Identity",
                table: "AspNetUsers",
                column: "BaseEntytiId",
                principalSchema: "Identity",
                principalTable: "BaseEntyti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BaseEntyti_BaseEntyti_ManagerId",
                schema: "Identity",
                table: "BaseEntyti",
                column: "ManagerId",
                principalSchema: "Identity",
                principalTable: "BaseEntyti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_BaseEntyti_BaseEntytiId",
                schema: "Identity",
                table: "Posts",
                column: "BaseEntytiId",
                principalSchema: "Identity",
                principalTable: "BaseEntyti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_BaseEntyti_EmployeeId",
                schema: "Identity",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_BaseEntyti_ManagerId",
                schema: "Identity",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BaseEntyti_BaseEntytiId",
                schema: "Identity",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BaseEntyti_BaseEntyti_ManagerId",
                schema: "Identity",
                table: "BaseEntyti");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_BaseEntyti_BaseEntytiId",
                schema: "Identity",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_BaseEntytiId",
                schema: "Identity",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaseEntyti",
                schema: "Identity",
                table: "BaseEntyti");

            migrationBuilder.DropIndex(
                name: "IX_BaseEntyti_ManagerId",
                schema: "Identity",
                table: "BaseEntyti");

            migrationBuilder.DropColumn(
                name: "BaseEntytiId",
                schema: "Identity",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "Identity",
                table: "BaseEntyti");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                schema: "Identity",
                table: "BaseEntyti");

            migrationBuilder.RenameTable(
                name: "BaseEntyti",
                schema: "Identity",
                newName: "Manager",
                newSchema: "Identity");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "Identity",
                table: "AspNetUsers",
                newName: "ManagerId");

            migrationBuilder.RenameColumn(
                name: "BaseEntytiId",
                schema: "Identity",
                table: "AspNetUsers",
                newName: "ManagerId1");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_BaseEntytiId",
                schema: "Identity",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_ManagerId1");

            migrationBuilder.AddColumn<string>(
                name: "EmployeesId",
                schema: "Identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeesId1",
                schema: "Identity",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Manager",
                schema: "Identity",
                table: "Manager",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Employee",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    ImageName = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    MobileNumber = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Occupation = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalSchema: "Identity",
                        principalTable: "Manager",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_EmployeeId",
                schema: "Identity",
                table: "Posts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ManagerId",
                schema: "Identity",
                table: "Posts",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeesId1",
                schema: "Identity",
                table: "AspNetUsers",
                column: "EmployeesId1");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ManagerId",
                schema: "Identity",
                table: "Employee",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Employee_EmployeeId",
                schema: "Identity",
                table: "Address",
                column: "EmployeeId",
                principalSchema: "Identity",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Manager_ManagerId",
                schema: "Identity",
                table: "Address",
                column: "ManagerId",
                principalSchema: "Identity",
                principalTable: "Manager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employee_EmployeesId1",
                schema: "Identity",
                table: "AspNetUsers",
                column: "EmployeesId1",
                principalSchema: "Identity",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Manager_ManagerId1",
                schema: "Identity",
                table: "AspNetUsers",
                column: "ManagerId1",
                principalSchema: "Identity",
                principalTable: "Manager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Employee_EmployeeId",
                schema: "Identity",
                table: "Posts",
                column: "EmployeeId",
                principalSchema: "Identity",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Manager_ManagerId",
                schema: "Identity",
                table: "Posts",
                column: "ManagerId",
                principalSchema: "Identity",
                principalTable: "Manager",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
