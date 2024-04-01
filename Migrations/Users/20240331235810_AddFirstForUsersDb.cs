using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationsCenter.Migrations.Users
{
    /// <inheritdoc />
    public partial class AddFirstForUsersDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationsModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Author = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Activity = table.Column<int>(type: "INTEGER", nullable: false),
                    Outline = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    FirstTimeCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Submited = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserModelId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationsModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationsModel_Users_UserModelId",
                        column: x => x.UserModelId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationsModel_UserModelId",
                table: "ApplicationsModel",
                column: "UserModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationsModel");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
