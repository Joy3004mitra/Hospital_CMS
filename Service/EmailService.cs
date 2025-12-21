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
                mail.From = new MailAddress(from, "Ratnakamal Medical Centre of Excellence");
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
    //            mail.From = new MailAddress(formEmail, "Ratnakamal Medical Centre of Excellence");
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

    public async Task<bool> SendContactMailAsync(ContactModel contactModel)
    {
        var smtpServer = _configuration["EmailSettings:SmtpServer"];
        int smtpPort = 587;
        var fromEmail = _configuration["EmailSettings:FromEmail"];
        var adminEmail = _configuration["EmailSettings:AdminEmail"];
        var encodedPassword = _configuration["EmailSettings:FromPassword"];
        var fromPassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPassword));

        string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "admin_contact_mail.html");
        string fileContent = File.ReadAllText(path);
        fileContent = fileContent.Replace("FULLNAME", contactModel.Name);
        fileContent = fileContent.Replace("EMAIL", contactModel.Email);
        fileContent = fileContent.Replace("PHONENO", contactModel.PhoneNumber);
        fileContent = fileContent.Replace("SUBJECT", contactModel.Subject);
        fileContent = fileContent.Replace("MESSAGE", contactModel.Message);
        fileContent = fileContent.Replace("DATETIME", DateTime.Now.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));


        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromEmail, "Ratnakamal Medical Centre of Excellence");
        mail.To.Add(adminEmail);
        mail.Subject = "New Contact Form Submission Received - " + contactModel.Name;
        mail.Body = fileContent;
        mail.IsBodyHtml = true;

        SmtpClient smtp = new SmtpClient(smtpServer, smtpPort);

        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(mail);

        path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "user_contact_mail.html");
        fileContent = File.ReadAllText(path);
        fileContent = fileContent.Replace("FULLNAME", contactModel.Name);
        fileContent = fileContent.Replace("EMAIL", contactModel.Email);
        fileContent = fileContent.Replace("PHONENO", contactModel.PhoneNumber);
        fileContent = fileContent.Replace("SUBJECT", contactModel.Subject);
        fileContent = fileContent.Replace("MESSAGE", contactModel.Message);
        //fileContent = fileContent.Replace("DATETIME", DateTime.Now.ToString());
        fileContent = fileContent.Replace("DATETIME", DateTime.Now.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));

        MailMessage usermail = new MailMessage();
        usermail.From = new MailAddress(fromEmail, "Ratnakamal Medical Centre of Excellence");
        usermail.To.Add(contactModel.Email);
        usermail.Subject = "Thank You for Contacting Ratnakamal Medical Centre of Excellence!";
        usermail.Body = fileContent;
        usermail.IsBodyHtml = true;

        SmtpClient usersmtp = new SmtpClient(smtpServer, smtpPort);

        usersmtp.UseDefaultCredentials = false;
        usersmtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
        usersmtp.EnableSsl = true;
        await smtp.SendMailAsync(usermail);

        return true;
    }

    public async Task<bool> SendBookingMailAsync(AppointmentModel appointmentModel)
    {
        var smtpServer = _configuration["EmailSettings:SmtpServer"];
        int smtpPort = 587;
        var fromEmail = _configuration["EmailSettings:FromEmail"];
        var adminEmail = _configuration["EmailSettings:AdminEmail"];
        var encodedPassword = _configuration["EmailSettings:FromPassword"];
        var fromPassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPassword));
        DateTime parsedDate;

        string path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "admin_booking_mail.html");
        string fileContent = File.ReadAllText(path);
        fileContent = fileContent.Replace("REQUESTSERVICE", appointmentModel.ServiceName);
        //fileContent = fileContent.Replace("DATETIME", appointmentModel.AppointmentDate);
        fileContent = fileContent.Replace("PATIENTNAME", appointmentModel.FullName);
        fileContent = fileContent.Replace("AGE", appointmentModel.Age);
        fileContent = fileContent.Replace("GENDER", appointmentModel.Gender);
        fileContent = fileContent.Replace("PHONENO", appointmentModel.PhoneNumber);
        fileContent = fileContent.Replace("EMAIL", appointmentModel.Email);
        fileContent = fileContent.Replace("DOCTORNAME", appointmentModel.DoctorName);
        fileContent = fileContent.Replace("TYPE", appointmentModel.PatientStatus);
        fileContent = fileContent.Replace("MESSAGE", appointmentModel.Message);

        if (DateTime.TryParse(appointmentModel.AppointmentDate, out parsedDate))
        {
            fileContent = fileContent.Replace("DATETIME", parsedDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        }

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromEmail, "Ratnakamal Medical Centre of Excellence");
        mail.To.Add(adminEmail);
        if (DateTime.TryParse(appointmentModel.AppointmentDate, out parsedDate))
        {
            mail.Subject = "Booking Request for" + " " + appointmentModel.ServiceName + " on " + parsedDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
        
        mail.Body = fileContent;
        mail.IsBodyHtml = true;

        SmtpClient smtp = new SmtpClient(smtpServer, smtpPort);

        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(mail);

        path = Path.Combine(Environment.CurrentDirectory, "Templates", "mail", "user_booking_mail.html");
        fileContent = File.ReadAllText(path);
        fileContent = fileContent.Replace("REQUESTSERVICE", appointmentModel.ServiceName);
        //fileContent = fileContent.Replace("DATETIME", appointmentModel.AppointmentDate);
        fileContent = fileContent.Replace("PATIENTNAME", appointmentModel.FullName);
        fileContent = fileContent.Replace("AGE", appointmentModel.Age);

        if (DateTime.TryParse(appointmentModel.AppointmentDate, out parsedDate))
        {
            fileContent = fileContent.Replace("DATETIME", parsedDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
        }

        MailMessage usermail = new MailMessage();
        usermail.From = new MailAddress(fromEmail, "Ratnakamal Medical Centre of Excellence");
        usermail.To.Add(appointmentModel.Email);
        usermail.Subject = "We Received your Booking Request for" + " " + appointmentModel.ServiceName + " at Hiramani Hospital";
        usermail.Body = fileContent;
        usermail.IsBodyHtml = true;

        SmtpClient usersmtp = new SmtpClient(smtpServer, smtpPort);

        usersmtp.UseDefaultCredentials = false;
        usersmtp.Credentials = new NetworkCredential(fromEmail, fromPassword);
        usersmtp.EnableSsl = true;
        await smtp.SendMailAsync(usermail);

        return true;
    }
}
