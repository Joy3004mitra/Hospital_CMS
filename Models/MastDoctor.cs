using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public partial class MastDoctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MastDoctorKey { get; set; }
        public string DoctorName { get; set; } = null!;
        public string? DoctorDegree { get; set; }
        public string? DoctorDesc { get; set; }
        public string? FaceboolProfileLink { get; set; }
        public string? LinkedInLink { get; set; }
        public string? XLink { get; set; }
        public string? DoctorImage { get; set; }
        public int? EntUserKey { get; set; }
        public DateTime EntDate { get; set; }
        public DateTime EntTime { get; set; }
        public int? EditUserKey { get; set; }
        public DateTime? EditDate { get; set; }
        public DateTime? EditTime { get; set; }
        public byte? TagActive { get; set; }
        public byte? TagDelete { get; set; }
    }
}
