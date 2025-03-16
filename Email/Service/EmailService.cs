using System;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void SendEmail(string to, string subject, string body)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Your Name", _config["SMTP:Username"]));
            email.To.Add(new MailboxAddress("Recipient", to));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["SMTP:Username"], _config["SMTP:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);

            Console.WriteLine($"[✔] Email sent successfully to {to}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[✖] Email sending failed: {ex.Message}");
            throw;
        }
    }
}
