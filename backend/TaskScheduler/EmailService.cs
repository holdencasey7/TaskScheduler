using System.Net;
using System.Net.Mail;
using Amazon.SecretsManager.Extensions.Caching;
using Newtonsoft.Json;

namespace TaskScheduler;

public class EmailService() : IEmailService
{
    private const string EmailCredentialsSecretName = "TaskScheduler/EmailCredentials";

    private static readonly SecretCacheConfiguration CacheConfiguration = new SecretCacheConfiguration
    {
        CacheItemTTL = 86400000
    };
    private readonly SecretsManagerCache _cache = new SecretsManagerCache(CacheConfiguration);
    
    public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string body)
    {
        var secretString = await _cache.GetSecretString(EmailCredentialsSecretName) ?? throw new Exception("Email credentials not configured.");
        var emailCredentials = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretString) ?? throw new Exception("Email credentials not deserialized.");
        var emailUser = emailCredentials["User"] ?? throw new Exception("Email user not found.");
        var emailPassword = emailCredentials["Password"] ?? throw new Exception("Email password not found.");
        
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(emailUser, emailPassword),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailUser),
            Subject = subject,
            Body = body,
            IsBodyHtml = false,
        };

        mailMessage.To.Add(recipientEmail);

        try
        {
            smtpClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
        
        return true;
    }
}