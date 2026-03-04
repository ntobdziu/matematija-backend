using MimeKit;

namespace MatematijaAPI.Services;

public interface IEmailService
{
    Task PosaljiVerifikacioniKod(string email, string kod);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task PosaljiVerifikacioniKod(string email, string kod)
    {
        var poruka = new MimeMessage();
        poruka.From.Add(new MailboxAddress("MatemaTI&JA", _config["Email:Od"]!));
        poruka.To.Add(new MailboxAddress("", email));
        poruka.Subject = "Verifikacioni kod - MatemaTI&JA";

        poruka.Body = new TextPart("html")
        {
            Text = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #7C3AED;'>Dobrodošao/la u MatemaTI&JA! 🎉</h2>
                    <p>Tvoj verifikacioni kod je:</p>
                    <div style='background: #EDE9FE; padding: 20px; text-align: center; border-radius: 8px; margin: 20px 0;'>
                        <h1 style='color: #7C3AED; font-size: 48px; margin: 0; letter-spacing: 8px;'>{kod}</h1>
                    </div>
                    <p style='color: #6B7280;'>Kod ističe za <strong>10 minuta</strong>.</p>
                    <p style='color: #6B7280; font-size: 14px;'>Ako nisi registrovao/la nalog, ignoriši ovaj email.</p>
                </div>
            "
        };

        using var klijent = new MailKit.Net.Smtp.SmtpClient();
        await klijent.ConnectAsync(
            _config["Email:Host"]!, 
            int.Parse(_config["Email:Port"]!), 
            MailKit.Security.SecureSocketOptions.StartTls
        );
        await klijent.AuthenticateAsync(
            _config["Email:Korisnik"]!, 
            _config["Email:Lozinka"]!
        );
        await klijent.SendAsync(poruka);
        await klijent.DisconnectAsync(true);
    }
}