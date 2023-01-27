using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulk.App.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "stadium",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stadium", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "team",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Initials = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FoundedAt = table.Column<DateTime>(name: "Founded_At", type: "datetime2", nullable: false),
                    StadiumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_team_stadium_StadiumId",
                        column: x => x.StadiumId,
                        principalTable: "stadium",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_stadium_Name",
                table: "stadium",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_team_Name_Country",
                table: "team",
                columns: new[] { "Name", "Country" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_team_StadiumId",
                table: "team",
                column: "StadiumId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "team");

            migrationBuilder.DropTable(
                name: "stadium");
        }
    }
}
