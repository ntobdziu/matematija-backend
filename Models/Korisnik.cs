namespace MatematijaAPI.Models;

public class Korisnik
{
    public int Id { get; set; }
    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LozinkaHash { get; set; } = string.Empty;
    public string Uloga { get; set; } = "Ucenik";
    public int? Razred { get; set; }
    public DateTime KreiranNa { get; set; } = DateTime.UtcNow;

    // Email verifikacija
    public bool EmailPotvrđen { get; set; } = false;
    public string? VerifikacioniKod { get; set; }
    public DateTime? KodIsticeNa { get; set; }

    // Navigaciona svojstva
    public ICollection<Rezervacija> Rezervacije { get; set; } = new List<Rezervacija>();
}