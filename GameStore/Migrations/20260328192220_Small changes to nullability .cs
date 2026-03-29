using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Migrations
{
    /// <inheritdoc />
    public partial class Smallchangestonullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SteamApps_Games_GameId",
                table: "SteamApps");

            migrationBuilder.DropIndex(
                name: "IX_SteamApps_GameId",
                table: "SteamApps");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "SteamApps",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Games",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_SteamApps_GameId",
                table: "SteamApps",
                column: "GameId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SteamApps_Games_GameId",
                table: "SteamApps",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SteamApps_Games_GameId",
                table: "SteamApps");

            migrationBuilder.DropIndex(
                name: "IX_SteamApps_GameId",
                table: "SteamApps");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "SteamApps",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Games",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SteamApps_GameId",
                table: "SteamApps",
                column: "GameId",
                unique: true,
                filter: "[GameId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SteamApps_Games_GameId",
                table: "SteamApps",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}
