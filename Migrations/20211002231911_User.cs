using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZADALKHAIR.Migrations
{
    public partial class User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserFirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserLastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    USerCountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRoleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserCreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
