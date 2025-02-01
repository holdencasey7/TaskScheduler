namespace TaskScheduler;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string recipientEmail, string subject, string body);
}