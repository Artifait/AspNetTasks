using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetTasks.Migrations
{
    /// <inheritdoc />
    public partial class DisableForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Disable foreign keys
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Re-enable foreign keys
            migrationBuilder.Sql("PRAGMA foreign_keys = ON;");
        }
    }
}
