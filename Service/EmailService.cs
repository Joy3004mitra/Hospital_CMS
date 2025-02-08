using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
    {
        try
        {
            // Read email settings from appsettings.json
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            int smtpPort = 587;//int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var formEmail = _configuration["EmailSettings:FromEmail"];
            var encodedPassword = _configuration["EmailSettings:FromPassword"];
            var formPassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPassword));

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(formEmail, "Hiramani Memorial Hospital");
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(formEmail, formPassword);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return false;
        }
    }
}
