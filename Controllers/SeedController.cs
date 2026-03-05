using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatematijaAPI.Data;
using MatematijaAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace MatematijaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly AppDbContext _db;

    public SeedController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("dodaj-termine-2026")]
    [AllowAnonymous] // PRIVREMENO - ukloni nakon testiranja
    public async Task<IActionResult> DodajTermine2026()
    {
        try
        {
            var termini = new List<Termin>
            {
                // MART 2026 (prošli)
                new() { DatumVreme = new DateTime(2026, 3, 1, 10, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 1, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 3, 1, 14, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 1, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 3, 2, 11, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 2, Aktivan = true },

                // MART 2026 (predstojeći)
                new() { DatumVreme = new DateTime(2026, 3, 6, 10, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 1, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 3, 6, 14, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 3, 7, 9, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 3, 7, 15, 0, 0), TipCasaId = 4, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 3, 8, 10, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 1, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 3, 9, 11, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 3, 10, 14, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },

                // APRIL 2026
                new() { DatumVreme = new DateTime(2026, 4, 5, 10, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 4, 5, 14, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 4, 6, 11, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 4, 7, 9, 0, 0), TipCasaId = 4, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 4, 8, 15, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 4, 9, 10, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 4, 10, 14, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 4, 12, 11, 0, 0), TipCasaId = 4, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 4, 13, 9, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 4, 14, 15, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },

                // MAJ 2026
                new() { DatumVreme = new DateTime(2026, 5, 3, 10, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 5, 3, 14, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 5, 4, 11, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 5, 5, 9, 0, 0), TipCasaId = 4, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 5, 6, 15, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 5, 7, 10, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 5, 8, 14, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 5, 10, 11, 0, 0), TipCasaId = 4, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 5, 11, 9, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 5, 12, 15, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },

                // JUN 2026
                new() { DatumVreme = new DateTime(2026, 6, 1, 10, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 6, 2, 14, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 6, 3, 11, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 6, 4, 9, 0, 0), TipCasaId = 4, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 6, 5, 15, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 6, 7, 10, 0, 0), TipCasaId = 3, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 6, 8, 14, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 6, 9, 11, 0, 0), TipCasaId = 4, MestaUkupno = 5, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 6, 10, 9, 0, 0), TipCasaId = 1, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },
                new() { DatumVreme = new DateTime(2026, 6, 11, 15, 0, 0), TipCasaId = 2, MestaUkupno = 1, MestaZauzeto = 0, Aktivan = true },

            };

            await _db.Termini.AddRangeAsync(termini);
            await _db.SaveChangesAsync();

            return Ok(new { poruka = $"Dodato {termini.Count} termina za 2026. godinu!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { greska = ex.Message });
        }
    }

    [HttpGet("dodaj-rezervacije-nejla")]
    [AllowAnonymous] // PRIVREMENO
    public async Task<IActionResult> DodajRezervacijeNejla()
    {
        try
        {
            var nejla = await _db.Korisnici.FirstOrDefaultAsync(k => k.Email == "nejla123@gmail.com");
            if (nejla == null)
                return NotFound(new { poruka = "Korisnik nejla123@gmail.com ne postoji!" });

            // Prošli časovi
            var termin1 = await _db.Termini.FirstOrDefaultAsync(t => t.DatumVreme == new DateTime(2026, 3, 1, 10, 0, 0));
            var termin2 = await _db.Termini.FirstOrDefaultAsync(t => t.DatumVreme == new DateTime(2026, 3, 1, 14, 0, 0));
            var termin3 = await _db.Termini.FirstOrDefaultAsync(t => t.DatumVreme == new DateTime(2026, 3, 2, 11, 0, 0));

            // Predstojeći časovi
            var termin4 = await _db.Termini.FirstOrDefaultAsync(t => t.DatumVreme == new DateTime(2026, 3, 6, 10, 0, 0));
            var termin5 = await _db.Termini.FirstOrDefaultAsync(t => t.DatumVreme == new DateTime(2026, 3, 8, 10, 0, 0));

            var rezervacije = new List<Rezervacija>();

            if (termin1 != null)
            {
                rezervacije.Add(new Rezervacija
                {
                    KorisnikId = nejla.Id,
                    TerminId = termin1.Id,
                    Status = "Završen",
                    Napomena = "Odličan čas! Naučila sam kvadratne jednačine.",
                    Ocena = 5,
                    KreiranaNa = new DateTime(2026, 2, 25, 10, 0, 0)
                });
            }

            if (termin2 != null)
            {
                rezervacije.Add(new Rezervacija
                {
                    KorisnikId = nejla.Id,
                    TerminId = termin2.Id,
                    Status = "Završen",
                    Napomena = "Vježbanje za test - super pomoć!",
                    Ocena = 5,
                    KreiranaNa = new DateTime(2026, 2, 25, 11, 0, 0)
                });
            }

            if (termin3 != null)
            {
                rezervacije.Add(new Rezervacija
                {
                    KorisnikId = nejla.Id,
                    TerminId = termin3.Id,
                    Status = "Završen",
                    Napomena = "Grupni čas - odlična atmosfera!",
                    Ocena = 4,
                    KreiranaNa = new DateTime(2026, 2, 26, 9, 0, 0)
                });
            }

            if (termin4 != null)
            {
                rezervacije.Add(new Rezervacija
                {
                    KorisnikId = nejla.Id,
                    TerminId = termin4.Id,
                    Status = "Aktivan",
                    Napomena = "Priprema za kontrolni iz algebre",
                    KreiranaNa = DateTime.UtcNow
                });
            }

            if (termin5 != null)
            {
                rezervacije.Add(new Rezervacija
                {
                    KorisnikId = nejla.Id,
                    TerminId = termin5.Id,
                    Status = "Aktivan",
                    Napomena = "Nastavak - kvadratne jednačine",
                    KreiranaNa = DateTime.UtcNow
                });
            }

            await _db.Rezervacije.AddRangeAsync(rezervacije);
            await _db.SaveChangesAsync();

            return Ok(new { poruka = $"Dodato {rezervacije.Count} rezervacija za Nejlu!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { greska = ex.Message });
        }
    }
}