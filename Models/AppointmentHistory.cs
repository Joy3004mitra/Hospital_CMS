using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public class AppointmentHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HeadAppointmentKey { get; set; }
        public string ServiceName { get; set; }
        public string DoctorName { get; set; }
        public string PatientStatus { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Message { get; set; }
        public DateTime EntDate { get; set; }
        public DateTime EntTime { get; set; }
        public byte? TagActive { get; set; }
        public byte? TagDelete { get; set; }
    }

}
