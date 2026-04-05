using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Migrations
{
    /// <inheritdoc />
    public partial class RemovalofDistributorTableaddingDeveloperproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributorGame");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Distributors",
                table: "Distributors");

            migrationBuilder.RenameTable(
                name: "Distributors",
                newName: "Distributor");

            migrationBuilder.AddColumn<string>(
                name: "Developer",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DistributorId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Distributor",
                table: "Distributor",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Developer", "DistributorId" },
                values: new object[] { null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Games_DistributorId",
                table: "Games",
                column: "DistributorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Distributor_DistributorId",
                table: "Games",
                column: "DistributorId",
                principalTable: "Distributor",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Distributor_DistributorId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_DistributorId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Distributor",
                table: "Distributor");

            migrationBuilder.DropColumn(
                name: "Developer",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "DistributorId",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "Distributor",
                newName: "Distributors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Distributors",
                table: "Distributors",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DistributorGame",
                columns: table => new
                {
                    DistributorsId = table.Column<int>(type: "int", nullable: false),
                    GamesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributorGame", x => new { x.DistributorsId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_DistributorGame_Distributors_DistributorsId",
                        column: x => x.DistributorsId,
                        principalTable: "Distributors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributorGame_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistributorGame_GamesId",
                table: "DistributorGame",
                column: "GamesId");
        }
    }
}
