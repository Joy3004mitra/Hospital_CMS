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

    public async Task<bool> SendEmailAsync(string toEmail, string adminSubject, string adminMessage)
    {
        try
        {
            // Read email settings from appsettings.json
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            int smtpPort = 587;
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var adminEmail = _configuration["EmailSettings:AdminEmail"];
            var encodedPassword = _configuration["EmailSettings:FromPassword"];
            var fromPassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPassword));

            // Define the admin's email subject & message
            string userSubject = "New User Enquiry Received";  // Custom subject for admin
            string userMessage = $"A new Enquiry was submitted by: {toEmail}\n\nMessage:\n{adminMessage}";

            // Send email to user
            bool userEmailSent = await SendSingleEmailAsync(toEmail, fromEmail, userSubject, userMessage, smtpServer, smtpPort, fromPassword);

            // Send email to admin
            bool adminEmailSent = await SendSingleEmailAsync(adminEmail, fromEmail, adminSubject, adminMessage, smtpServer, smtpPort, fromPassword);

            // Return true only if both emails are sent successfully
            if (userEmailSent && adminEmailSent)
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending emails: {ex.Message}");
            return false;
        }
    }

    // Helper method to send a single email
    private async Task<bool> SendSingleEmailAsync(string to, string from, string subject, string body, string smtpServer, int smtpPort, string password)
    {
        try
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(from, "Hiramani Memorial Hospital");
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(from, password);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);  // Using async version
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email to {to}: {ex.Message}");
            return false;
        }
    }

    //public async Task<bool> SendEmailAsync(string toEmail, string subject, string message)
    //{
    //    try
    //    {
    //        // Read email settings from appsettings.json
    //        var smtpServer = _configuration["EmailSettings:SmtpServer"];
    //        int smtpPort = 587;//int.Parse(_configuration["EmailSettings:SmtpPort"]);
    //        var formEmail = _configuration["EmailSettings:FromEmail"];
    //        var adminEmail = _configuration["EmailSettings:AdminEmail"];
    //        var encodedPassword = _configuration["EmailSettings:FromPassword"];
    //        var formPassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPassword));

    //        using (MailMessage mail = new MailMessage())
    //        {
    //            mail.From = new MailAddress(formEmail, "Hiramani Memorial Hospital");
    //            mail.To.Add(toEmail);
    //            mail.Subject = subject;
    //            mail.Body = message;
    //            mail.IsBodyHtml = true;

    //            using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
    //            {
    //                smtp.UseDefaultCredentials = false;
    //                smtp.Credentials = new NetworkCredential(formEmail, formPassword);
    //                smtp.EnableSsl = true;
    //                smtp.Send(mail);
    //                return true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error sending email: {ex.Message}");
    //        return false;
    //    }
    //}
}
