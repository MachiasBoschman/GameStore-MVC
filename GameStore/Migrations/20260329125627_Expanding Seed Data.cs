using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Migrations
{
    /// <inheritdoc />
    public partial class ExpandingSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SteamApps",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Distributors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Microsoft Store" },
                    { 4, "Playstation" },
                    { 5, "Xbox" }
                });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                column: "SteamAppId",
                value: 1938090);

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Xbox" });

            migrationBuilder.InsertData(
                table: "SteamApps",
                columns: new[] { "Id", "GameId" },
                values: new object[] { 1938090, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Distributors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Distributors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Distributors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SteamApps",
                keyColumn: "Id",
                keyValue: 1938090);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                column: "SteamAppId",
                value: 1);

            migrationBuilder.InsertData(
                table: "SteamApps",
                columns: new[] { "Id", "GameId" },
                values: new object[] { 1, 1 });
        }
    }
}
