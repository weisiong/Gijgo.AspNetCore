using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gijgo.AspNetCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Checked = table.Column<bool>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false),
                    Population = table.Column<long>(nullable: true),
                    FlagUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Locations_Locations_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PlaceOfBirth = table.Column<string>(nullable: true),
                    CountryID = table.Column<int>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "Date", nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Players_Locations_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerTeams",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlayerID = table.Column<int>(nullable: false),
                    FromYear = table.Column<int>(nullable: false),
                    ToYear = table.Column<int>(nullable: false),
                    Team = table.Column<string>(nullable: true),
                    Apps = table.Column<int>(nullable: false),
                    Goals = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTeams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlayerTeams_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "ID", "Checked", "FlagUrl", "Name", "OrderNumber", "ParentID", "Population" },
                values: new object[,]
                {
                    { 1, false, null, "Asia", 1, null, null },
                    { 5, false, null, "North America", 2, null, null },
                    { 11, false, null, "South America", 3, null, null },
                    { 15, false, null, "Europe", 4, null, null }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "ID", "Checked", "FlagUrl", "Name", "OrderNumber", "ParentID", "Population" },
                values: new object[,]
                {
                    { 2, false, "http://code.gijgo.com/flags/24/China.png", "China", 1, 1, 1373541278L },
                    { 3, false, "http://code.gijgo.com/flags/24/Japan.png", "Japan", 2, 1, 126730000L },
                    { 4, false, "http://code.gijgo.com/flags/24/Mongolia.png", "Mongolia", 3, 1, 3081677L },
                    { 6, false, "http://code.gijgo.com/flags/24/United%20States%20of%20America(USA).png", "USA", 1, 5, 325145963L },
                    { 9, false, "http://code.gijgo.com/flags/24/canada.png", "Canada", 2, 5, 35151728L },
                    { 10, false, "http://code.gijgo.com/flags/24/mexico.png", "Mexico", 3, 5, 119530753L },
                    { 12, false, "http://code.gijgo.com/flags/24/brazil.png", "Brazil", 1, 11, 207350000L },
                    { 13, false, "http://code.gijgo.com/flags/24/argentina.png", "Argentina", 2, 11, 43417000L },
                    { 14, false, "http://code.gijgo.com/flags/24/colombia.png", "Colombia", 3, 11, 49819638L },
                    { 16, false, "http://code.gijgo.com/flags/24/england.png", "England", 1, 15, 54786300L },
                    { 17, false, "http://code.gijgo.com/flags/24/germany.png", "Germany", 2, 15, 82175700L },
                    { 18, false, "http://code.gijgo.com/flags/24/bulgaria.png", "Bulgaria", 3, 15, 7101859L },
                    { 19, false, "http://code.gijgo.com/flags/24/poland.png", "Poland", 4, 15, 38454576L }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "ID", "Checked", "FlagUrl", "Name", "OrderNumber", "ParentID", "Population" },
                values: new object[,]
                {
                    { 7, false, null, "California", 1, 6, 39144818L },
                    { 8, false, null, "Florida", 2, 6, 20271272L }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "ID", "CountryID", "DateOfBirth", "IsActive", "Name", "OrderNumber", "PlaceOfBirth" },
                values: new object[,]
                {
                    { 2, 12, new DateTime(1976, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Ronaldo Luís Nazário de Lima", 2, "Rio de Janeiro, Brazil" },
                    { 5, 14, new DateTime(1991, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "James Rodríguez", 5, "Cúcuta, Colombia" },
                    { 3, 16, new DateTime(1966, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "David Platt", 3, "Chadderton, Lancashire, England" },
                    { 4, 17, new DateTime(1986, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Manuel Neuer", 4, "Gelsenkirchen, West Germany" },
                    { 1, 18, new DateTime(1966, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Hristo Stoichkov", 1, "Plovdiv, Bulgaria" },
                    { 6, 18, new DateTime(1981, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Dimitar Berbatov", 6, "Blagoevgrad, Bulgaria" },
                    { 7, 19, new DateTime(1988, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Robert Lewandowski", 7, "Warsaw, Poland" }
                });

            migrationBuilder.InsertData(
                table: "PlayerTeams",
                columns: new[] { "ID", "Apps", "FromYear", "Goals", "PlayerID", "Team", "ToYear" },
                values: new object[,]
                {
                    { 11, 14, 1993, 12, 2, "Cruzeiro", 1994 },
                    { 8, 27, 1998, 12, 1, "Kashiwa Reysol", 1999 },
                    { 7, 2, 1998, 1, 1, "Al-Nassr", 1998 },
                    { 6, 4, 1998, 1, 1, "CSKA Sofia", 1998 },
                    { 5, 26, 1996, 7, 1, "Barcelona", 1998 },
                    { 4, 23, 1995, 5, 1, "Parma", 1996 },
                    { 3, 149, 1990, 77, 1, "Barcelona", 1995 },
                    { 2, 119, 1984, 81, 1, "CSKA Sofia", 1990 },
                    { 1, 32, 1982, 14, 1, "Hebros Harmanli", 1984 },
                    { 24, 5, 1999, 1, 3, "Nottingham Forest", 2001 },
                    { 23, 88, 1995, 13, 3, "Arsenal", 1998 },
                    { 22, 55, 1993, 17, 3, "Sampdoria", 1995 },
                    { 21, 16, 1992, 3, 3, "Juventus", 1993 },
                    { 20, 29, 1991, 11, 3, "Bari", 1992 },
                    { 19, 121, 1988, 50, 3, "Aston Villa", 1991 },
                    { 18, 134, 1985, 56, 3, "Crewe Alexandra", 1988 },
                    { 17, 31, 2009, 18, 2, "Corinthians", 2011 },
                    { 16, 20, 2007, 9, 2, "Milan", 2008 },
                    { 15, 127, 2002, 83, 2, "Real Madrid", 2007 },
                    { 14, 68, 1997, 49, 2, "Inter Milan", 2002 },
                    { 13, 37, 1996, 34, 2, "Barcelona", 1997 },
                    { 12, 46, 1994, 42, 2, "PSV", 1996 },
                    { 9, 51, 2000, 17, 1, "Chicago Fire", 2002 },
                    { 10, 21, 2003, 5, 1, "D.C. United", 2003 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ParentID",
                table: "Locations",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CountryID",
                table: "Players",
                column: "CountryID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTeams_PlayerID",
                table: "PlayerTeams",
                column: "PlayerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerTeams");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
