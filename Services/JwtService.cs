using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MatematijaAPI.Models;

namespace MatematijaAPI.Services;

public interface IJwtService
{
    string GenerisiToken(Korisnik korisnik);
}

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerisiToken(Korisnik korisnik)
    {
        var kljuc = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Kljuc"]!)
        );

        var kredencijali = new SigningCredentials(kljuc, SecurityAlgorithms.HmacSha256);

        var tvrdnje = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, korisnik.Id.ToString()),
            new Claim(ClaimTypes.Email, korisnik.Email),
            new Claim(ClaimTypes.Name, $"{korisnik.Ime} {korisnik.Prezime}"),
            new Claim(ClaimTypes.Role, korisnik.Uloga)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Izdavac"],
            audience: _config["Jwt:Primaoc"],
            claims: tvrdnje,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: kredencijali
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
