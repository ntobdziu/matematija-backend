using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MatematijaAPI.Data;
using MatematijaAPI.DTOs;

namespace MatematijaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class KorisniciController : ControllerBase
{
    private readonly AppDbContext _db;

    public KorisniciController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/korisnici/profil
    // Vraca podatke o ulogovanom korisniku
    [HttpGet("profil")]
    public async Task<IActionResult> GetProfil()
    {
        var idStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var id = int.Parse(idStr!);

        var korisnik = await _db.Korisnici.FindAsync(id);
        if (korisnik == null)
            return NotFound();

        return Ok(new KorisnikDto
        {
            Id = korisnik.Id,
            Ime = korisnik.Ime,
            Prezime = korisnik.Prezime,
            Email = korisnik.Email,
            Uloga = korisnik.Uloga,
            Razred = korisnik.Razred
        });
    }

    // GET api/korisnici  (samo Admin)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSvi()
    {
        var korisnici = await _db.Korisnici
            .Select(k => new KorisnikDto
            {
                Id = k.Id,
                Ime = k.Ime,
                Prezime = k.Prezime,
                Email = k.Email,
                Uloga = k.Uloga,
                Razred = k.Razred
            })
            .ToListAsync();

        return Ok(korisnici);
    }

    // DELETE api/korisnici/{id}  (samo Admin)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Obrisi(int id)
    {
        var korisnik = await _db.Korisnici.FindAsync(id);
        if (korisnik == null)
            return NotFound(new { poruka = "Korisnik nije pronađen." });

        // Ne dozvoljavamo brisanje admin naloga
        if (korisnik.Uloga == "Admin")
            return BadRequest(new { poruka = "Ne može se obrisati admin nalog." });

        _db.Korisnici.Remove(korisnik);
        await _db.SaveChangesAsync();

        return Ok(new { poruka = "Korisnik obrisan." });
    }
}
