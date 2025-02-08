using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Models
{
    public partial class MastHosService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MastHosServiceKey { get; set; }
        public string ServiceName { get; set; } = null!;
        public string? ServiceShortDesc { get; set; }
        public string? ServiceImage { get; set; }
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
