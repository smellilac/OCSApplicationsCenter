using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class НазваниеМиграции : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Author = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Activity = table.Column<int>(type: "integer", nullable: false),
                    Outline = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FirstTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Submited = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnSubmittedApps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserModelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Author = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Activity = table.Column<int>(type: "integer", nullable: false),
                    Outline = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FirstTimeCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Submited = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnSubmittedApps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnSubmittedApps_Users_UserModelId",
                        column: x => x.UserModelId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnSubmittedApps_UserModelId",
                table: "UnSubmittedApps",
                column: "UserModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "UnSubmittedApps");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
