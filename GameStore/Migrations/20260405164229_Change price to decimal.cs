using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.Migrations
{
    /// <inheritdoc />
    public partial class Changepricetodecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Distributor_DistributorId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Distributor");

            migrationBuilder.DropIndex(
                name: "IX_Games_DistributorId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "DistributorId",
                table: "Games");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Games",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 50m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Games",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "DistributorId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Distributor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributor", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Distributor",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Steam" },
                    { 2, "Epic Games" },
                    { 3, "Microsoft Store" },
                    { 4, "Playstation" },
                    { 5, "Xbox" }
                });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DistributorId", "Price" },
                values: new object[] { null, 50.0 });

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
    }
}
