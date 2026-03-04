namespace MatematijaAPI.Models;

public class TipCasa
{
    public int Id { get; set; }
    public string Naziv { get; set; } = string.Empty;         // "Individualni 1-4. razred"
    public string Tip { get; set; } = string.Empty;           // "Individualni" ili "Grupni"
    public string Razred { get; set; } = string.Empty;        // "1-4. razred" ili "5-8. razred"
    public decimal Cena { get; set; }
    public int TrajanjeMinuta { get; set; }
    public string Opis { get; set; } = string.Empty;
    public int MaxUcenika { get; set; } = 1;                  // 1 za individualni, 5 za grupni
    public bool Aktivan { get; set; } = true;

    // Teme koje se obradjuju - cuva se kao JSON string
    public string TemeJson { get; set; } = "[]";

    // Navigaciona svojstva
    public ICollection<Termin> Termini { get; set; } = new List<Termin>();
}
