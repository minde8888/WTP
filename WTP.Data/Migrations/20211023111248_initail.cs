using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WTP.Data.Migrations
{
    public partial class initail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Roles = table.Column<string>(type: "text", nullable: true),
                    AcceptTerms = table.Column<bool>(type: "boolean", nullable: false),
                    VerificationToken = table.Column<string>(type: "text", nullable: true),
                    Verified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ResetToken = table.Column<string>(type: "text", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PasswordReset = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "Identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: true),
                    ProviderKey = table.Column<string>(type: "text", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "Identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Manager",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    Occupation = table.Column<string>(type: "text", nullable: true),
                    MobileNumber = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    ImageName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manager", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manager_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    Expires = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    JwtId = table.Column<string>(type: "text", nullable: true),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Revoked = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    Occupation = table.Column<string>(type: "text", nullable: true),
                    MobileNumber = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    ImageName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employee_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalSchema: "Identity",
                        principalTable: "Manager",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<long>(type: "bigint", nullable: false),
                    Zip = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Identity",
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Address_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalSchema: "Identity",
                        principalTable: "Manager",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                schema: "Identity",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Context = table.Column<string>(type: "text", nullable: true),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ImageName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Identity",
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Manager_ManagerId",
                        column: x => x.ManagerId,
                        principalSchema: "Identity",
                        principalTable: "Manager",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_EmployeeId",
                schema: "Identity",
                table: "Address",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Address_ManagerId",
                schema: "Identity",
                table: "Address",
                column: "ManagerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "Identity",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Identity",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "Identity",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "Identity",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "Identity",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Identity",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Identity",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_ManagerId",
                schema: "Identity",
                table: "Employee",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UserId",
                schema: "Identity",
                table: "Employee",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manager_UserId",
                schema: "Identity",
                table: "Manager",
                column: "UserId",
                unique: true);

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
                name: "IX_RefreshToken_UserId",
                schema: "Identity",
                table: "RefreshToken",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Posts",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Employee",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Manager",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "Identity");
        }
    }
}
