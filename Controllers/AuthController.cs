using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatematijaAPI.Data;
using MatematijaAPI.DTOs;
using MatematijaAPI.Models;
using MatematijaAPI.Services;

namespace MatematijaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IJwtService _jwt;
    private readonly IEmailService _emailService;

    public AuthController(AppDbContext db, IJwtService jwt, IEmailService emailService)
    {
        _db = db;
        _jwt = jwt;
        _emailService = emailService;
    }

    // POST api/auth/registracija
    [HttpPost("registracija")]
    public async Task<IActionResult> Registracija([FromBody] RegistracijaDto dto)
    {
        // Provjera da li email vec postoji
        var postojiEmail = await _db.Korisnici.AnyAsync(k => k.Email == dto.Email);
        if (postojiEmail)
            return BadRequest(new { poruka = "Korisnik sa ovom email adresom već postoji." });

        // Validacija
        if (dto.Lozinka.Length < 6)
            return BadRequest(new { poruka = "Lozinka mora imati najmanje 6 karaktera." });

        // Hashiranje lozinke
        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Lozinka);

        // Generiši 6-cifreni kod
        var kod = new Random().Next(100000, 999999).ToString();

        var korisnik = new Korisnik
        {
            Ime = dto.Ime,
            Prezime = dto.Prezime,
            Email = dto.Email.ToLower().Trim(),
            LozinkaHash = hash,
            Uloga = dto.Uloga == "Admin" ? "Admin" : (dto.Uloga == "Profesor" ? "Profesor" : "Ucenik"),
            Razred = dto.Razred,
            EmailPotvrđen = true
            //VerifikacioniKod = kod,
            //KodIsticeNa = DateTime.UtcNow.AddMinutes(10)
        };

        _db.Korisnici.Add(korisnik);
        await _db.SaveChangesAsync();

        // Pošalji email sa verifikacionim kodom
        /*  
        try
        {
            await _emailService.PosaljiVerifikacioniKod(korisnik.Email, kod);
        }
        catch (Exception ex)
        {
            // Loguj grešku, ali ne prekidaj registraciju
            Console.WriteLine($"Greška pri slanju emaila: {ex.Message}");
        }
        */
        var token = _jwt.GenerisiToken(korisnik);

        return Ok(new AuthOdgovorDto
        {
            Token = token,
            Korisnik = new KorisnikDto
            {
                Id = korisnik.Id,
                Ime = korisnik.Ime,
                Prezime = korisnik.Prezime,
                Email = korisnik.Email,
                Uloga = korisnik.Uloga,
                Razred = korisnik.Razred
            }
        });
    }

    // POST api/auth/verifikuj-email
    [HttpPost("verifikuj-email")]
    public async Task<IActionResult> VerifikujEmail([FromBody] VerifikujEmailDto dto)
    {
        var korisnik = await _db.Korisnici.FindAsync(dto.KorisnikId);

        if (korisnik == null)
            return NotFound(new { poruka = "Korisnik nije pronađen." });

        if (korisnik.EmailPotvrđen)
            return BadRequest(new { poruka = "Email je već potvrđen." });

        if (korisnik.VerifikacioniKod != dto.Kod)
            return BadRequest(new { poruka = "Pogrešan kod." });

        if (korisnik.KodIsticeNa < DateTime.UtcNow)
            return BadRequest(new { poruka = "Kod je istekao. Zatraži novi." });

        // Potvrdi email
        korisnik.EmailPotvrđen = true;
        korisnik.VerifikacioniKod = null;
        korisnik.KodIsticeNa = null;
        await _db.SaveChangesAsync();

        var token = _jwt.GenerisiToken(korisnik);

        return Ok(new AuthOdgovorDto
        {
            Token = token,
            Korisnik = new KorisnikDto
            {
                Id = korisnik.Id,
                Ime = korisnik.Ime,
                Prezime = korisnik.Prezime,
                Email = korisnik.Email,
                Uloga = korisnik.Uloga,
                Razred = korisnik.Razred
            }
        });
    }

    // POST api/auth/prijava
    [HttpPost("prijava")]
    public async Task<IActionResult> Prijava([FromBody] PrijavaDto dto)
    {
        var korisnik = await _db.Korisnici
            .FirstOrDefaultAsync(k => k.Email == dto.Email.ToLower().Trim());

        if (korisnik == null)
            return Unauthorized(new { poruka = "Pogrešan email ili lozinka." });

        var lozinkaOk = BCrypt.Net.BCrypt.Verify(dto.Lozinka, korisnik.LozinkaHash);
        if (!lozinkaOk)
            return Unauthorized(new { poruka = "Pogrešan email ili lozinka." });

        // Provjeri da li je email potvrđen
        /*
        if (!korisnik.EmailPotvrđen)
            return Unauthorized(new
            {
                poruka = "Email nije potvrđen. Provjeri inbox za verifikacioni kod.",
                korisnikId = korisnik.Id,
                trebaVerifikacija = true
            });
        */
        var token = _jwt.GenerisiToken(korisnik);

        return Ok(new AuthOdgovorDto
        {
            Token = token,
            Korisnik = new KorisnikDto
            {
                Id = korisnik.Id,
                Ime = korisnik.Ime,
                Prezime = korisnik.Prezime,
                Email = korisnik.Email,
                Uloga = korisnik.Uloga,
                Razred = korisnik.Razred
            }
        });
    }
}