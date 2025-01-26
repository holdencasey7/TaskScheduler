using System.Net;
using System.Net.Mail;

namespace TaskScheduler;

public class EmailService : IEmailService
{
    public void SendEmail(string recipientEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("me@holdencasey.com", "mduz hhkz azdo mnlo"),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("taskscheduler@holdencasey.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
        };

        mailMessage.To.Add(recipientEmail);

        smtpClient.Send(mailMessage);
    }
}