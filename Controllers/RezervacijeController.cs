using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MatematijaAPI.Data;
using MatematijaAPI.DTOs;
using MatematijaAPI.Models;

namespace MatematijaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Sve rute zahtijevaju prijavu
public class RezervacijeController : ControllerBase
{
    private readonly AppDbContext _db;

    public RezervacijeController(AppDbContext db)
    {
        _db = db;
    }

    // Pomocna metoda - uzima ID ulogovanog korisnika iz JWT tokena
    private int GetKorisnikId()
    {
        var idTvrdnja = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(idTvrdnja!);
    }

    // GET api/rezervacije/moje
    // Vraca sve rezervacije ulogovanog korisnika
    [HttpGet("moje")]
    public async Task<IActionResult> GetMoje()
    {
        var korisnikId = GetKorisnikId();

        var rezervacije = await _db.Rezervacije
            .Include(r => r.Termin)
                .ThenInclude(t => t.TipCasa)
            .Where(r => r.KorisnikId == korisnikId)
            .OrderByDescending(r => r.Termin.DatumVreme)
            .ToListAsync();

        var rezultat = rezervacije.Select(r => new RezervacijaDto
        {
            Id = r.Id,
            TerminId = r.TerminId,
            DatumVreme = r.Termin.DatumVreme,
            TipCasa = r.Termin.TipCasa.Tip,
            Razred = r.Termin.TipCasa.Razred,
            Status = r.Status,
            Napomena = r.Napomena,
            Ocena = r.Ocena,
            KreiranaNa = r.KreiranaNa
        });

        return Ok(rezultat);
    }

    // GET api/rezervacije/sve  (samo za profesora i admina)
    // Vraca sve rezervacije
    [HttpGet("sve")]
    [Authorize(Roles = "Profesor,Admin")]
    public async Task<IActionResult> GetSve()
    {
        var rezervacije = await _db.Rezervacije
            .Include(r => r.Termin).ThenInclude(t => t.TipCasa)
            .Include(r => r.Korisnik)
            .OrderByDescending(r => r.Termin.DatumVreme)
            .ToListAsync();

        var rezultat = rezervacije.Select(r => new
        {
            r.Id,
            KorisnikIme = r.Korisnik.Ime,           // ← DODAJ
            KorisnikPrezime = r.Korisnik.Prezime,   // ← DODAJ
            KorisnikRazred = r.Korisnik.Razred,
            Korisnik = $"{r.Korisnik.Ime} {r.Korisnik.Prezime}",
            Email = r.Korisnik.Email,
            DatumVreme = r.Termin.DatumVreme,
            TipCasa = r.Termin.TipCasa.Naziv,
            r.Status,
            r.Napomena
        });

        return Ok(rezultat);
    }

    // POST api/rezervacije
    // Korisnik zakazuje cas
    [HttpPost]
    [Authorize(Roles = "Ucenik,Admin")]
    public async Task<IActionResult> Zakaži([FromBody] KreirajRezervacijuDto dto)
    {
        var korisnikId = GetKorisnikId();

        // Provjeri da li termin postoji i ima slobodnih mesta
        var termin = await _db.Termini
            .Include(t => t.TipCasa)
            .FirstOrDefaultAsync(t => t.Id == dto.TerminId && t.Aktivan);

        if (termin == null)
            return NotFound(new { poruka = "Termin nije pronađen." });

        if (!termin.ImaSlobodnihMesta)
            return BadRequest(new { poruka = "Termin je popunjen." });

        // Provjeri da korisnik nije vec rezervisao isti termin
        var vecRezervisan = await _db.Rezervacije.AnyAsync(r =>
            r.KorisnikId == korisnikId &&
            r.TerminId == dto.TerminId &&
            r.Status != "Otkazan");

        if (vecRezervisan)
            return BadRequest(new { poruka = "Već si rezervisao/la ovaj termin." });

        // Kreiraj rezervaciju
        var rezervacija = new Rezervacija
        {
            KorisnikId = korisnikId,
            TerminId = dto.TerminId,
            Napomena = dto.Napomena,
            Status = "Predstojeći"
        };

        // Povecaj broj zauzetih mesta
        termin.MestaZauzeto++;

        _db.Rezervacije.Add(rezervacija);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            poruka = "Čas uspješno zakazan!",
            rezervacijaId = rezervacija.Id,
            datum = termin.DatumVreme,
            tipCasa = termin.TipCasa.Naziv
        });
    }

    // DELETE api/rezervacije/{id}
    // Korisnik otkazuje svoju rezervaciju (mora biti 48h unaprijed)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Otkaži(int id)
    {
        var korisnikId = GetKorisnikId();
        var uloga = User.FindFirst(ClaimTypes.Role)?.Value;

        var rezervacija = await _db.Rezervacije
            .Include(r => r.Termin)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (rezervacija == null)
            return NotFound(new { poruka = "Rezervacija nije pronađena." });

        // Samo vlasnik ili admin mogu otkazati
        if (rezervacija.KorisnikId != korisnikId && uloga != "Admin")
            return Forbid();

        if (rezervacija.Status != "Predstojeći")
            return BadRequest(new { poruka = "Može se otkazati samo predstojeći čas." });

        // Provjera 48h
        var satDoTermina = (rezervacija.Termin.DatumVreme - DateTime.UtcNow).TotalHours;
        if (satDoTermina < 48 && uloga != "Admin")
            return BadRequest(new { poruka = "Čas se može otkazati najmanje 48 sati ranije." });

        // Otkazivanje
        rezervacija.Status = "Otkazan";
        rezervacija.OtkazanaNa = DateTime.UtcNow;

        // Oslobodi mjesto
        rezervacija.Termin.MestaZauzeto = Math.Max(0, rezervacija.Termin.MestaZauzeto - 1);

        await _db.SaveChangesAsync();

        return Ok(new { poruka = "Rezervacija otkazana." });
    }

    // PUT api/rezervacije/{id}/ocena
    // Korisnik ocjenjuje odrzan cas
    [HttpPut("{id}/ocena")]
    [Authorize(Roles = "Ucenik,Admin")]
    public async Task<IActionResult> DajOcenu(int id, [FromBody] OceniRezervacijuDto dto)
    {
        var korisnikId = GetKorisnikId();

        if (dto.Ocena < 1 || dto.Ocena > 5)
            return BadRequest(new { poruka = "Ocena mora biti između 1 i 5." });

        var rezervacija = await _db.Rezervacije
            .Include(r => r.Termin)
            .FirstOrDefaultAsync(r => r.Id == id && r.KorisnikId == korisnikId);

        if (rezervacija == null)
            return NotFound(new { poruka = "Rezervacija nije pronađena." });

        if (rezervacija.Status != "Završen")
            return BadRequest(new { poruka = "Može se ocijeniti samo završen čas." });

        rezervacija.Ocena = dto.Ocena;
        await _db.SaveChangesAsync();

        return Ok(new { poruka = "Ocena sačuvana!" });
    }
}
