using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationsCenter.Migrations.SubmittedAppsDbMigrations
{
    /// <inheritdoc />
    public partial class AddThirdTimeCreatedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Author = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Activity = table.Column<int>(type: "INTEGER", nullable: false),
                    Outline = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    FirstTimeCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Submited = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
