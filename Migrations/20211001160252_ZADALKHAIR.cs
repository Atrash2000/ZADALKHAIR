using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZADALKHAIR.Migrations
{
    public partial class ZADALKHAIR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ContactID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContactPhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ContactCounrty = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContactSubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactMassege = table.Column<string>(type: "nvarchar(765)", maxLength: 765, nullable: false),
                    ContactSatuts = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SatutsUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");
        }
    }
}
