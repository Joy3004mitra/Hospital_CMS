using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HospitalManagement.Models
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HeadSetting> HeadSetting { get; set; } = null!;
        public virtual DbSet<MastDoctor> MastDoctors { get; set; } = null!;
        public virtual DbSet<MastHosService> MastHosServices { get; set; } = null!;
        public virtual DbSet<MastHosTestimonials> MastHosTestimonials { get; set; } = null!;
        public virtual DbSet<AppointmentHistory> AppointmentHistories { get; set; } = null!;
        public virtual DbSet<AdminLogin> AdminLogin { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=SANA\\MSSQLSERVER01;Database=HospitalDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HeadSetting>(entity =>
            {
                entity.HasKey(e => e.HeadSettingKey);

                entity.ToTable("HEAD_SETTING");

                entity.Property(e => e.HeadSettingKey).HasColumnName("HEAD_SETTING_KEY");

                entity.Property(e => e.EditDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("EDIT_DATE");

                entity.Property(e => e.EditTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("EDIT_TIME");

                entity.Property(e => e.EditUserKey).HasColumnName("EDIT_USER_KEY");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL_ID");

                entity.Property(e => e.EmgncyPhoneNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMGNCY_PHONE_NO");

                entity.Property(e => e.EntDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("ENT_DATE");

                entity.Property(e => e.EntTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("ENT_TIME");

                entity.Property(e => e.EntUserKey).HasColumnName("ENT_USER_KEY");

                entity.Property(e => e.LinkedInLink)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("LINKED_IN_LINK");

                entity.Property(e => e.LogoImage)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("LOGO_IMAGE");

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PHONE_NO");

                entity.Property(e => e.SiteAddress)
                    .IsUnicode(false)
                    .HasColumnName("SITE_ADDRESS");

                entity.Property(e => e.TagActive).HasColumnName("TAG_ACTIVE");

                entity.Property(e => e.TagDelete).HasColumnName("TAG_DELETE");
            });

            modelBuilder.Entity<MastDoctor>(entity =>
            {
                entity.HasKey(e => e.MastDoctorKey);

                entity.ToTable("MAST_DOCTOR");

                entity.Property(e => e.MastDoctorKey).HasColumnName("MAST_DOCTOR_KEY");

                entity.Property(e => e.DoctorDegree)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DOCTOR_DEGREE");

                entity.Property(e => e.DoctorDesc)
                    .IsUnicode(false)
                    .HasColumnName("DOCTOR_DESC");

                entity.Property(e => e.DoctorImage)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DOCTOR_IMAGE");

                entity.Property(e => e.DoctorName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DOCTOR_NAME");

                entity.Property(e => e.EditDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("EDIT_DATE");

                entity.Property(e => e.EditTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("EDIT_TIME");

                entity.Property(e => e.EditUserKey).HasColumnName("EDIT_USER_KEY");

                entity.Property(e => e.EntDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("ENT_DATE");

                entity.Property(e => e.EntTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("ENT_TIME");

                entity.Property(e => e.EntUserKey).HasColumnName("ENT_USER_KEY");

                entity.Property(e => e.FaceboolProfileLink)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("FACEBOOL_PROFILE_LINK");

                entity.Property(e => e.LinkedInLink)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("LINKED_IN_LINK");

                entity.Property(e => e.TagActive).HasColumnName("TAG_ACTIVE");

                entity.Property(e => e.TagDelete).HasColumnName("TAG_DELETE");

                entity.Property(e => e.XLink)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("X_LINK");
            });

            modelBuilder.Entity<MastHosService>(entity =>
            {
                entity.HasKey(e => e.MastHosServiceKey);

                entity.ToTable("MAST_HOS_SERVICE");

                entity.Property(e => e.MastHosServiceKey).HasColumnName("MAST_HOS_SERVICE_KEY");

                entity.Property(e => e.EditDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("EDIT_DATE");

                entity.Property(e => e.EditTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("EDIT_TIME");

                entity.Property(e => e.EditUserKey).HasColumnName("EDIT_USER_KEY");

                entity.Property(e => e.EntDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("ENT_DATE");

                entity.Property(e => e.EntTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("ENT_TIME");

                entity.Property(e => e.EntUserKey).HasColumnName("ENT_USER_KEY");

                entity.Property(e => e.ServiceImage)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SERVICE_IMAGE");

                entity.Property(e => e.ServiceName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SERVICE_NAME");

                entity.Property(e => e.ServiceShortDesc)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SERVICE_SHORT_DESC");

                entity.Property(e => e.TagActive).HasColumnName("TAG_ACTIVE");

                entity.Property(e => e.TagDelete).HasColumnName("TAG_DELETE");
            });

            modelBuilder.Entity<MastHosTestimonials>(entity =>
            {
                entity.HasKey(e => e.MastHosTestimonialsKey);

                entity.ToTable("MAST_HOS_TESTIMONIALS");

                entity.Property(e => e.MastHosTestimonialsKey).HasColumnName("MAST_HOS_TESTIMONIALS_KEY");

                entity.Property(e => e.EditDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("EDIT_DATE");

                entity.Property(e => e.EditTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("EDIT_TIME");

                entity.Property(e => e.EditUserKey).HasColumnName("EDIT_USER_KEY");

                entity.Property(e => e.EntDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("ENT_DATE");

                entity.Property(e => e.EntTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("ENT_TIME");

                entity.Property(e => e.EntUserKey).HasColumnName("ENT_USER_KEY");

                entity.Property(e => e.PatientComments)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("PATIENT_COMMENTS");

                entity.Property(e => e.PatientImage)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PATIENT_IMAGE");

                entity.Property(e => e.PatientName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PATIENT_NAME");

                entity.Property(e => e.TagActive).HasColumnName("TAG_ACTIVE");

                entity.Property(e => e.TagDelete).HasColumnName("TAG_DELETE");
            });

            modelBuilder.Entity<AppointmentHistory>(entity =>
            {
                entity.HasKey(e => e.HeadAppointmentKey);

                entity.ToTable("APPOINTMENT_HISTORY");

                entity.Property(e => e.HeadAppointmentKey).HasColumnName("HEAD_APPOINTMENT_KEY");

                entity.Property(e => e.ServiceName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SERVICE_NAME");

                entity.Property(e => e.DoctorName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DOCTOR_NAME");

                entity.Property(e => e.PatientStatus)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("PATIENT_STATUS");

                entity.Property(e => e.FullName)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME");

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GENDER");

                entity.Property(e => e.Age)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("AGE");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PHONE_NUMBER");

                entity.Property(e => e.AppointmentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("APPOINTMENT_DATE");

                entity.Property(e => e.Message)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasColumnName("MESSAGE");

                entity.Property(e => e.EntDate)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("ENT_DATE");

                entity.Property(e => e.EntTime)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("ENT_TIME");
                entity.Property(e => e.TagActive).HasColumnName("TAG_ACTIVE");

                entity.Property(e => e.TagDelete).HasColumnName("TAG_DELETE");
            });

            modelBuilder.Entity<AdminLogin>(entity =>
            {
                entity.HasKey(e => e.ID);

                entity.ToTable("ADMIN_LOGIN");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL_ID");

                entity.Property(e => e.Passwd)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PASSWD");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
