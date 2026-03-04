using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MatematijaAPI.Data;
using MatematijaAPI.DTOs;
using MatematijaAPI.Models;

namespace MatematijaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TerminiController : ControllerBase
{
    private readonly AppDbContext _db;

    public TerminiController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/termini?datum=2025-02-20
    // Vraca slobodne termine za odredjeni datum - dostupno svima
    [HttpGet]
    public async Task<IActionResult> GetZaDatum([FromQuery] DateTime? datum)
    {
        var query = _db.Termini
            .Include(t => t.TipCasa)
            .Where(t => t.Aktivan);

        // Ako je dat datum, filtriraj po njemu
        if (datum.HasValue)
        {
            var dan = datum.Value.Date;
            query = query.Where(t => t.DatumVreme.Date == dan);
        }
        else
        {
            // Ako datum nije dat, vrati termine od danas pa na dalje
            query = query.Where(t => t.DatumVreme >= DateTime.UtcNow);
        }

        var termini = await query
            .OrderBy(t => t.DatumVreme)
            .ToListAsync();

        var rezultat = termini.Select(t => new TerminDto
        {
            Id = t.Id,
            DatumVreme = t.DatumVreme,
            TipCasaId = t.TipCasaId,
            TipCasaNaziv = t.TipCasa.Naziv,
            MestaUkupno = t.MestaUkupno,
            MestaZauzeto = t.MestaZauzeto,
            ImaSlobodnihMesta = t.ImaSlobodnihMesta
        });

        return Ok(rezultat);
    }

    // GET api/termini/slobodni?mesec=2025-02
    // Vraca sve dane u mesecu koji imaju slobodnih termina
    [HttpGet("slobodni")]
    public async Task<IActionResult> GetSlobodniDani([FromQuery] string? mesec)
    {
        DateTime pocetak, kraj;

        if (mesec != null && DateTime.TryParse(mesec + "-01", out var parsovanMesec))
        {
            pocetak = parsovanMesec;
            kraj = parsovanMesec.AddMonths(1);
        }
        else
        {
            pocetak = DateTime.UtcNow.Date;
            kraj = pocetak.AddMonths(1);
        }

        var slobodniDani = await _db.Termini
            .Where(t => t.Aktivan
                && t.DatumVreme >= pocetak
                && t.DatumVreme < kraj
                && t.MestaZauzeto < t.MestaUkupno)
            .Select(t => t.DatumVreme.Date)
            .Distinct()
            .ToListAsync();

        return Ok(slobodniDani.Select(d => d.ToString("yyyy-MM-dd")));
    }

    // POST api/termini
    // Samo Profesor ili Admin moze da doda termin
    [HttpPost]
    [Authorize(Roles = "Profesor,Admin")]
    public async Task<IActionResult> Dodaj([FromBody] KreirajTerminDto dto)
    {
        // Provjera da li vec postoji termin u isto vreme za isti tip
        var postojiTermin = await _db.Termini.AnyAsync(t =>
            t.DatumVreme == dto.DatumVreme &&
            t.TipCasaId == dto.TipCasaId &&
            t.Aktivan);

        if (postojiTermin)
            return BadRequest(new { poruka = "Termin u ovo vreme već postoji." });

        var tipCasa = await _db.TipoviCasova.FindAsync(dto.TipCasaId);
        if (tipCasa == null)
            return NotFound(new { poruka = "Tip časa nije pronađen." });

        var termin = new Termin
        {
            DatumVreme = dto.DatumVreme,
            TipCasaId = dto.TipCasaId,
            MestaUkupno = tipCasa.MaxUcenika
        };

        _db.Termini.Add(termin);
        await _db.SaveChangesAsync();

        return Ok(new { poruka = "Termin uspješno dodan.", id = termin.Id });
    }

    // DELETE api/termini/{id}
    // Samo Profesor ili Admin moze da obrise termin
    [HttpDelete("{id}")]
    [Authorize(Roles = "Profesor,Admin")]
    public async Task<IActionResult> Obrisi(int id)
    {
        var termin = await _db.Termini.FindAsync(id);
        if (termin == null)
            return NotFound(new { poruka = "Termin nije pronađen." });

        // Provjeri da li ima aktivnih rezervacija
        var imaRezervacija = await _db.Rezervacije
            .AnyAsync(r => r.TerminId == id && r.Status == "Predstojeći");

        if (imaRezervacija)
            return BadRequest(new { poruka = "Ne možeš obrisati termin koji ima aktivnih rezervacija." });

        termin.Aktivan = false;
        await _db.SaveChangesAsync();

        return Ok(new { poruka = "Termin deaktiviran." });
    }
}
