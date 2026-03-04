using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MatematijaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMySQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ime = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prezime = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LozinkaHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Uloga = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Razred = table.Column<int>(type: "int", nullable: true),
                    KreiranNa = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EmailPotvrđen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VerifikacioniKod = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KodIsticeNa = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoviCasova",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tip = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Razred = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cena = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TrajanjeMinuta = table.Column<int>(type: "int", nullable: false),
                    Opis = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaxUcenika = table.Column<int>(type: "int", nullable: false),
                    Aktivan = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TemeJson = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoviCasova", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Termini",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DatumVreme = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TipCasaId = table.Column<int>(type: "int", nullable: false),
                    MestaUkupno = table.Column<int>(type: "int", nullable: false),
                    MestaZauzeto = table.Column<int>(type: "int", nullable: false),
                    Aktivan = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termini", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Termini_TipoviCasova_TipCasaId",
                        column: x => x.TipCasaId,
                        principalTable: "TipoviCasova",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rezervacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    TerminId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KreiranaNa = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OtkazanaNa = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Napomena = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ocena = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Termini_TerminId",
                        column: x => x.TerminId,
                        principalTable: "Termini",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Korisnici",
                columns: new[] { "Id", "Email", "EmailPotvrđen", "Ime", "KodIsticeNa", "KreiranNa", "LozinkaHash", "Prezime", "Razred", "Uloga", "VerifikacioniKod" },
                values: new object[,]
                {
                    { 1, "admin@matematija.rs", false, "Admin", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy", "Sistem", null, "Admin", null },
                    { 2, "profesor@matematija.rs", false, "Profesor", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy", "Matematike", null, "Profesor", null }
                });

            migrationBuilder.InsertData(
                table: "TipoviCasova",
                columns: new[] { "Id", "Aktivan", "Cena", "MaxUcenika", "Naziv", "Opis", "Razred", "TemeJson", "Tip", "TrajanjeMinuta" },
                values: new object[,]
                {
                    { 1, true, 800m, 1, "Individualni 1-4. razred", "Individualni čas za učenike od 1. do 4. razreda. Osnove matematike, sabiranje, oduzimanje, množenje i deljenje.", "1-4. razred", "[\"Sabiranje i oduzimanje\",\"Množenje i deljenje\",\"Geometrija\",\"Zadaci iz teksta\"]", "Individualni", 45 },
                    { 2, true, 1000m, 1, "Individualni 5-8. razred", "Individualni čas za učenike od 5. do 8. razreda. Algebarski izrazi, geometrija, jednačine i nejednačine.", "5-8. razred", "[\"Algebarski izrazi\",\"Jednačine\",\"Pitagorina teorema\",\"Statistika\"]", "Individualni", 60 },
                    { 3, true, 500m, 5, "Grupni 1-4. razred", "Grupni čas (do 5 učenika) za učenike od 1. do 4. razreda. Zabavno učenje u grupi!", "1-4. razred", "[\"Osnove aritmetike\",\"Geometrijski oblici\",\"Merne jedinice\",\"Matematičke igre\"]", "Grupni", 60 },
                    { 4, true, 700m, 5, "Grupni 5-8. razred", "Grupni čas (do 5 učenika) za učenike od 5. do 8. razreda. Tim rad i razmena znanja!", "5-8. razred", "[\"Racionalni brojevi\",\"Funkcije\",\"Trigonometrija\",\"Sistemi jednačina\"]", "Grupni", 90 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_Email",
                table: "Korisnici",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_KorisnikId_TerminId",
                table: "Rezervacije",
                columns: new[] { "KorisnikId", "TerminId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_TerminId",
                table: "Rezervacije",
                column: "TerminId");

            migrationBuilder.CreateIndex(
                name: "IX_Termini_TipCasaId",
                table: "Termini",
                column: "TipCasaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rezervacije");

            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "Termini");

            migrationBuilder.DropTable(
                name: "TipoviCasova");
        }
    }
}
