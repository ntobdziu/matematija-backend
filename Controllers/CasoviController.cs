using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MatematijaAPI.Data;
using MatematijaAPI.DTOs;
using MatematijaAPI.Models;

namespace MatematijaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CasoviController : ControllerBase
{
    private readonly AppDbContext _db;

    public CasoviController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/casovi
    // Vraca sve aktivne tipove casova - dostupno svima (neulogovanim takodje)
    [HttpGet]
    public async Task<IActionResult> GetSvi()
    {
        var tipovi = await _db.TipoviCasova
            .Where(t => t.Aktivan)
            .ToListAsync();

        var rezultat = tipovi.Select(t => new TipCasaDto
        {
            Id = t.Id,
            Naziv = t.Naziv,
            Tip = t.Tip,
            Razred = t.Razred,
            Cena = t.Cena,
            TrajanjeMinuta = t.TrajanjeMinuta,
            Opis = t.Opis,
            MaxUcenika = t.MaxUcenika,
            Teme = JsonSerializer.Deserialize<List<string>>(t.TemeJson) ?? new List<string>()
        });

        return Ok(rezultat);
    }

    // GET api/casovi/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetJedan(int id)
    {
        var tip = await _db.TipoviCasova.FindAsync(id);
        if (tip == null || !tip.Aktivan)
            return NotFound(new { poruka = "Tip časa nije pronađen." });

        return Ok(new TipCasaDto
        {
            Id = tip.Id,
            Naziv = tip.Naziv,
            Tip = tip.Tip,
            Razred = tip.Razred,
            Cena = tip.Cena,
            TrajanjeMinuta = tip.TrajanjeMinuta,
            Opis = tip.Opis,
            MaxUcenika = tip.MaxUcenika,
            Teme = JsonSerializer.Deserialize<List<string>>(tip.TemeJson) ?? new List<string>()
        });
    }

    // POST api/casovi
    // Samo Admin moze da doda novi tip casa
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Dodaj([FromBody] TipCasaDto dto)
    {
        var tip = new TipCasa
        {
            Naziv = dto.Naziv,
            Tip = dto.Tip,
            Razred = dto.Razred,
            Cena = dto.Cena,
            TrajanjeMinuta = dto.TrajanjeMinuta,
            Opis = dto.Opis,
            MaxUcenika = dto.MaxUcenika,
            TemeJson = JsonSerializer.Serialize(dto.Teme)
        };

        _db.TipoviCasova.Add(tip);
        await _db.SaveChangesAsync();

        return Ok(new { poruka = "Tip časa uspješno dodan.", id = tip.Id });
    }

    // DELETE api/casovi/{id}
    // Samo Admin moze da obrise (deaktivira) tip casa
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Obrisi(int id)
    {
        var tip = await _db.TipoviCasova.FindAsync(id);
        if (tip == null)
            return NotFound(new { poruka = "Tip časa nije pronađen." });

        // Soft delete - ne brisemo iz baze, samo deaktiviramo
        tip.Aktivan = false;
        await _db.SaveChangesAsync();

        return Ok(new { poruka = "Tip časa je deaktiviran." });
    }
}
