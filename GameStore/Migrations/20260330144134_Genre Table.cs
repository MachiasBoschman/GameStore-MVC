using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Migrations
{
    /// <inheritdoc />
    public partial class GenreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameGenre",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    GenresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenre", x => new { x.GamesId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_GameGenre_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Action" },
                    { 2, "Adventure" },
                    { 3, "Casual" },
                    { 4, "Indie" },
                    { 5, "Massively Multiplayer" },
                    { 6, "Racing" },
                    { 7, "RPG" },
                    { 8, "Simulation" },
                    { 9, "Sports" },
                    { 10, "Strategy" },
                    { 11, "Free To Play" },
                    { 12, "First Person Shooter" },
                    { 13, "Third Person Shooter" },
                    { 14, "Battle Royale" },
                    { 15, "MOBA" },
                    { 16, "Roguelike" },
                    { 17, "Metroidvania" },
                    { 18, "Soulslike" },
                    { 19, "Tower Defense" },
                    { 20, "Turn Based Strategy" },
                    { 21, "Real Time Strategy" },
                    { 22, "Survival" },
                    { 23, "Horror" },
                    { 24, "Puzzle" },
                    { 25, "Platformer" },
                    { 26, "Fighting" },
                    { 27, "Stealth" },
                    { 28, "Open World" },
                    { 29, "Visual Novel" },
                    { 30, "City Builder" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameGenre_GenresId",
                table: "GameGenre",
                column: "GenresId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGenre");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
