using MatematijaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace MatematijaAPI.Data;

public static class SeedData
{
    public static async Task Initialize(AppDbContext db)
    {
        // Primijeni migracije automatski
        await db.Database.MigrateAsync();

        // Ako već ima korisnika, preskoči seed
        if (await db.Korisnici.AnyAsync()) return;

        // 1. TIPOVI ČASOVA
        var tipoviCasova = new List<TipCasa>
        {
            new TipCasa
            {
                Naziv = "Individualni 1-4. razred",
                Opis = "Individualni časovi za učenike osnovne škole (1-4. razred)",
                Tip = "Individualni",
                Razred = "1-4",
                Cena = 800,
                TrajanjeMinuta = 45,
                MaxUcenika = 1,
                Aktivan = true
            },
            new TipCasa
            {
                Naziv = "Individualni 5-8. razred",
                Opis = "Individualni časovi za učenike osnovne škole (5-8. razred)",
                Tip = "Individualni",
                Razred = "5-8",
                Cena = 1000,
                TrajanjeMinuta = 60,
                MaxUcenika = 1,
                Aktivan = true
            },
            new TipCasa
            {
                Naziv = "Grupni 1-4. razred",
                Opis = "Grupni časovi za učenike osnovne škole (1-4. razred)",
                Tip = "Grupni",
                Razred = "1-4",
                Cena = 500,
                TrajanjeMinuta = 45,
                MaxUcenika = 5,
                Aktivan = true
            },
            new TipCasa
            {
                Naziv = "Grupni 5-8. razred",
                Opis = "Grupni časovi za učenike osnovne škole (5-8. razred)",
                Tip = "Grupni",
                Razred = "5-8",
                Cena = 600,
                TrajanjeMinuta = 60,
                MaxUcenika = 5,
                Aktivan = true
            }
        };
        await db.TipoviCasova.AddRangeAsync(tipoviCasova);
        await db.SaveChangesAsync();

        // 2. KORISNICI
        var korisnici = new List<Korisnik>
        {
            new Korisnik
            {
                Ime = "Admin",
                Prezime = "Sistem",
                Email = "admin@matematija.rs",
                LozinkaHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Uloga = "Admin",
                EmailPotvrđen = true,
                KreiranNa = DateTime.UtcNow
            },
            new Korisnik
            {
                Ime = "Profesor",
                Prezime = "Matematike",
                Email = "profesor@matematija.rs",
                LozinkaHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Uloga = "Profesor",
                EmailPotvrđen = true,
                KreiranNa = DateTime.UtcNow
            },
            new Korisnik
            {
                Ime = "Ana",
                Prezime = "Petrović",
                Email = "ana@mail.com",
                LozinkaHash = BCrypt.Net.BCrypt.HashPassword("test123"),
                Uloga = "Ucenik",
                Razred = 5,
                EmailPotvrđen = true,
                KreiranNa = DateTime.UtcNow
            },
            new Korisnik
            {
                Ime = "Marko",
                Prezime = "Marković",
                Email = "marko@mail.com",
                LozinkaHash = BCrypt.Net.BCrypt.HashPassword("test123"),
                Uloga = "Ucenik",
                Razred = 3,
                EmailPotvrđen = true,
                KreiranNa = DateTime.UtcNow
            },
            new Korisnik
            {
                Ime = "Jelena",
                Prezime = "Jovanović",
                Email = "jelena@mail.com",
                LozinkaHash = BCrypt.Net.BCrypt.HashPassword("test123"),
                Uloga = "Ucenik",
                Razred = 7,
                EmailPotvrđen = true,
                KreiranNa = DateTime.UtcNow
            }
        };
        await db.Korisnici.AddRangeAsync(korisnici);
        await db.SaveChangesAsync();

        // 3. TERMINI
        var termini = new List<Termin>
        {
            new Termin { DatumVreme = new DateTime(2026, 3, 10, 10, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
            new Termin { DatumVreme = new DateTime(2026, 3, 10, 14, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
            new Termin { DatumVreme = new DateTime(2026, 3, 11, 11, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
            new Termin { DatumVreme = new DateTime(2026, 3, 12, 9, 0, 0), TipCasaId = 4, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
            new Termin { DatumVreme = new DateTime(2026, 3, 13, 15, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
            new Termin { DatumVreme = new DateTime(2026, 3, 14, 10, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
            new Termin { DatumVreme = new DateTime(2026, 3, 17, 10, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
            new Termin { DatumVreme = new DateTime(2026, 3, 17, 14, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
            new Termin { DatumVreme = new DateTime(2026, 3, 18, 11, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
            new Termin { DatumVreme = new DateTime(2026, 3, 19, 9, 0, 0), TipCasaId = 4, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
            new Termin { DatumVreme = new DateTime(2026, 4, 5, 10, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
            new Termin { DatumVreme = new DateTime(2026, 4, 5, 14, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 2, Aktivan = true }
        };
        await db.Termini.AddRangeAsync(termini);
        await db.SaveChangesAsync();

        // 4. REZERVACIJE (primer)
        var rezervacije = new List<Rezervacija>
        {
            new Rezervacija
            {
                KorisnikId = korisnici[2].Id, // Ana
                TerminId = termini[0].Id,
                Status = "Predstojeći",
                Napomena = "Priprema za test",
                KreiranaNa = DateTime.UtcNow
            },
            new Rezervacija
            {
                KorisnikId = korisnici[3].Id, // Marko
                TerminId = termini[2].Id,
                Status = "Predstojeći",
                Napomena = "Vježbe za geometriju",
                KreiranaNa = DateTime.UtcNow
            }
        };
        await db.Rezervacije.AddRangeAsync(rezervacije);

        // Ažuriraj zauzeta mjesta
        termini[0].MestaZauzeto = 1;
        termini[2].MestaZauzeto = 1;

        await db.SaveChangesAsync();
    }
}