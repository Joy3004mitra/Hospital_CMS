using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public partial class MastBlog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MastBlogKey { get; set; }

        [Required]
        public string BlogTitle { get; set; } = null!;

        // short summary
        public string? BlogSummary { get; set; }

        // full content (can be HTML)
        public string? BlogContent { get; set; }

        // SEO fields
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        public string? BlogImage { get; set; }

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
