namespace MatematijaAPI.Models;

public class Termin
{
    public int Id { get; set; }

    // Datum i vreme pocetka
    public DateTime DatumVreme { get; set; }

    public int TipCasaId { get; set; }
    public TipCasa TipCasa { get; set; } = null!;

    // Koliko mesta ima ukupno (1 ili 5)
    public int MestaUkupno { get; set; } = 1;

    // Koliko je zauzeto
    public int MestaZauzeto { get; set; } = 0;

    // Da li je termin aktivan (profesor moze da ga deaktivira)
    public bool Aktivan { get; set; } = true;

    // Navigaciona svojstva
    public ICollection<Rezervacija> Rezervacije { get; set; } = new List<Rezervacija>();

    // Computed property - da li ima slobodnih mesta
    public bool ImaSlobodnihMesta => Aktivan && MestaZauzeto < MestaUkupno;
}
