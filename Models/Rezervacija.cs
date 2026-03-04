namespace MatematijaAPI.Models;

public class Rezervacija
{
    public int Id { get; set; }

    public int KorisnikId { get; set; }
    public Korisnik Korisnik { get; set; } = null!;

    public int TerminId { get; set; }
    public Termin Termin { get; set; } = null!;

    // "Predstojeći", "Završen", "Otkazan"
    public string Status { get; set; } = "Predstojeći";

    public DateTime KreiranaNa { get; set; } = DateTime.UtcNow;

    // Kada je otkazana (ako je otkazana)
    public DateTime? OtkazanaNa { get; set; }

    // Napomena korisnika pri zakazivanju
    public string? Napomena { get; set; }

    // Ocena (1-5) koju ucenik da posle casa
    public int? Ocena { get; set; }
}
