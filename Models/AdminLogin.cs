using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class AdminLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string EmailId { get; set; } = null!;
        public string? Passwd { get; set; }
    }
}
