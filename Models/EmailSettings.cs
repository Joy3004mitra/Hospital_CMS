namespace HospitalManagement.Models
{
    public class EmailSettings
    {
        public const string Path = "EmailSettings";
        public string SmtpServer { get; set; } = string.Empty;
        public string SmtpPort { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string FromPassword { get; set; } = string.Empty;
    }
}