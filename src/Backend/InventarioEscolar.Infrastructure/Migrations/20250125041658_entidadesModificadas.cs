using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarioEscolar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class entidadesModificadas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_RoomLocations_CategoryId",
                table: "Assets");

            migrationBuilder.AlterColumn<long>(
                name: "RoomLocationId",
                table: "Assets",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_RoomLocationId",
                table: "Assets",
                column: "RoomLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_RoomLocations_RoomLocationId",
                table: "Assets",
                column: "RoomLocationId",
                principalTable: "RoomLocations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_RoomLocations_RoomLocationId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_RoomLocationId",
                table: "Assets");

            migrationBuilder.AlterColumn<long>(
                name: "RoomLocationId",
                table: "Assets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_RoomLocations_CategoryId",
                table: "Assets",
                column: "CategoryId",
                principalTable: "RoomLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
