using Microsoft.EntityFrameworkCore;
using MatematijaAPI.Models;

namespace MatematijaAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Korisnik> Korisnici { get; set; }
    public DbSet<TipCasa> TipoviCasova { get; set; }
    public DbSet<Termin> Termini { get; set; }
    public DbSet<Rezervacija> Rezervacije { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Korisnik - email mora biti jedinstven
        modelBuilder.Entity<Korisnik>()
            .HasIndex(k => k.Email)
            .IsUnique();

        // Rezervacija - jedan korisnik ne moze dva puta da rezervise isti termin
        modelBuilder.Entity<Rezervacija>()
            .HasIndex(r => new { r.KorisnikId, r.TerminId })
            .IsUnique();

        // TipCasa -> Termini (jedan tip casa ima vise termina)
        modelBuilder.Entity<Termin>()
            .HasOne(t => t.TipCasa)
            .WithMany(tc => tc.Termini)
            .HasForeignKey(t => t.TipCasaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Rezervacija -> Korisnik
        modelBuilder.Entity<Rezervacija>()
            .HasOne(r => r.Korisnik)
            .WithMany(k => k.Rezervacije)
            .HasForeignKey(r => r.KorisnikId)
            .OnDelete(DeleteBehavior.Restrict);

        // Rezervacija -> Termin
        modelBuilder.Entity<Rezervacija>()
            .HasOne(r => r.Termin)
            .WithMany(t => t.Rezervacije)
            .HasForeignKey(r => r.TerminId)
            .OnDelete(DeleteBehavior.Restrict);

        // Precision za cenu
        modelBuilder.Entity<TipCasa>()
            .Property(t => t.Cena)
            .HasColumnType("decimal(10,2)");

        // Seed data - tipovi casova
        modelBuilder.Entity<TipCasa>().HasData(
            new TipCasa
            {
                Id = 1,
                Naziv = "Individualni 1-4. razred",
                Tip = "Individualni",
                Razred = "1-4. razred",
                Cena = 800,
                TrajanjeMinuta = 45,
                MaxUcenika = 1,
                Opis = "Individualni čas za učenike od 1. do 4. razreda. Osnove matematike, sabiranje, oduzimanje, množenje i deljenje.",
                TemeJson = "[\"Sabiranje i oduzimanje\",\"Množenje i deljenje\",\"Geometrija\",\"Zadaci iz teksta\"]"
            },
            new TipCasa
            {
                Id = 2,
                Naziv = "Individualni 5-8. razred",
                Tip = "Individualni",
                Razred = "5-8. razred",
                Cena = 1000,
                TrajanjeMinuta = 60,
                MaxUcenika = 1,
                Opis = "Individualni čas za učenike od 5. do 8. razreda. Algebarski izrazi, geometrija, jednačine i nejednačine.",
                TemeJson = "[\"Algebarski izrazi\",\"Jednačine\",\"Pitagorina teorema\",\"Statistika\"]"
            },
            new TipCasa
            {
                Id = 3,
                Naziv = "Grupni 1-4. razred",
                Tip = "Grupni",
                Razred = "1-4. razred",
                Cena = 500,
                TrajanjeMinuta = 60,
                MaxUcenika = 5,
                Opis = "Grupni čas (do 5 učenika) za učenike od 1. do 4. razreda. Zabavno učenje u grupi!",
                TemeJson = "[\"Osnove aritmetike\",\"Geometrijski oblici\",\"Merne jedinice\",\"Matematičke igre\"]"
            },
            new TipCasa
            {
                Id = 4,
                Naziv = "Grupni 5-8. razred",
                Tip = "Grupni",
                Razred = "5-8. razred",
                Cena = 700,
                TrajanjeMinuta = 90,
                MaxUcenika = 5,
                Opis = "Grupni čas (do 5 učenika) za učenike od 5. do 8. razreda. Tim rad i razmena znanja!",
                TemeJson = "[\"Racionalni brojevi\",\"Funkcije\",\"Trigonometrija\",\"Sistemi jednačina\"]"
            }
        );

        // Seed data - admin korisnik
        // Lozinka: Admin123! (BCrypt hash)
        modelBuilder.Entity<Korisnik>().HasData(
            new Korisnik
            {
                Id = 1,
                Ime = "Admin",
                Prezime = "Sistem",
                Email = "admin@matematija.rs",
                LozinkaHash = "$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy",
                Uloga = "Admin",
                KreiranNa = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Korisnik
            {
                Id = 2,
                Ime = "Profesor",
                Prezime = "Matematike",
                Email = "profesor@matematija.rs",
                LozinkaHash = "$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy",
                Uloga = "Profesor",
                KreiranNa = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
