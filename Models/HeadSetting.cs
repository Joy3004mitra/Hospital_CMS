using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public partial class HeadSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HeadSettingKey { get; set; }
        public string EmailId { get; set; } = null!;
        public string? SiteAddress { get; set; }
        public string? PhoneNo { get; set; }
        public string? EmgncyPhoneNo { get; set; }
        public string? LinkedInLink { get; set; }
        public string? LogoImage { get; set; }
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
