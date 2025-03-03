using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioEscolar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class uniquepatrimony : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Assets_PatrimonyCode",
                table: "Assets",
                column: "PatrimonyCode",
                unique: true,
                filter: "[PatrimonyCode] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assets_PatrimonyCode",
                table: "Assets");
        }
    }
}
