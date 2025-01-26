using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace TaskScheduler;

public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly IConfiguration _configuration = configuration;

    public void SendEmail(string recipientEmail, string subject, string body)
    {
        var emailUser = _configuration["Email:User"] ?? throw new Exception("Email user not configured.");
        var emailPassword = _configuration["Email:Password"] ?? throw new Exception("Email password not configured.");
        
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(emailUser, emailPassword),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailUser ?? "noreply@gmail.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
        };

        mailMessage.To.Add(recipientEmail);

        smtpClient.Send(mailMessage);
    }
}