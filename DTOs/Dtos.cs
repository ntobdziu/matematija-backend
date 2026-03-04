namespace MatematijaAPI.DTOs;

// ===== AUTH =====

public class RegistracijaDto
{
    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Lozinka { get; set; } = string.Empty;
    public string Uloga { get; set; } = "Ucenik";
    public int? Razred { get; set; }
}

public class PrijavaDto
{
    public string Email { get; set; } = string.Empty;
    public string Lozinka { get; set; } = string.Empty;
}

public class AuthOdgovorDto
{
    public string Token { get; set; } = string.Empty;
    public KorisnikDto Korisnik { get; set; } = null!;
}

// DODAJ OVO OVDJE - NOVI DTO
public class VerifikujEmailDto
{
    public int KorisnikId { get; set; }
    public string Kod { get; set; } = string.Empty;
}

// ===== KORISNIK =====

public class KorisnikDto
{
    public int Id { get; set; }
    public string Ime { get; set; } = string.Empty;
    public string Prezime { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Uloga { get; set; } = string.Empty;
    public int? Razred { get; set; }
}

// ===== TIPOVI CASOVA =====

public class TipCasaDto
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;
    public string Tip { get; set; } = string.Empty;
    public string Razred { get; set; } = string.Empty;
    public decimal Cena { get; set; }
    public int TrajanjeMinuta { get; set; }
    public string Opis { get; set; } = string.Empty;
    public int MaxUcenika { get; set; }
    public List<string> Teme { get; set; } = new();
}

// ===== TERMINI =====

public class TerminDto
{
    public int Id { get; set; }
    public DateTime DatumVreme { get; set; }
    public int TipCasaId { get; set; }
    public string TipCasaNaziv { get; set; } = string.Empty;
    public int MestaUkupno { get; set; }
    public int MestaZauzeto { get; set; }
    public bool ImaSlobodnihMesta { get; set; }
}

public class KreirajTerminDto
{
    public DateTime DatumVreme { get; set; }
    public int TipCasaId { get; set; }
}

// ===== REZERVACIJE =====

public class KreirajRezervacijuDto
{
    public int TerminId { get; set; }
    public string? Napomena { get; set; }
}

public class RezervacijaDto
{
    public int Id { get; set; }
    public string KorisnikIme { get; set; } = string.Empty;
    public string KorisnikPrezime { get; set; } = string.Empty;
    public int? KorisnikRazred { get; set; }
    public int TerminId { get; set; }
    public DateTime DatumVreme { get; set; }
    public string TipCasa { get; set; } = string.Empty;
    public string Razred { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Napomena { get; set; }
    public int? Ocena { get; set; }
    public DateTime KreiranaNa { get; set; }
}

public class OceniRezervacijuDto
{
    public int Ocena { get; set; }
}
