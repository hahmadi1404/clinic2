using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QCS_Config.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Agreement> Agreements { get; set; } = null!;
        public virtual DbSet<Bill> Bills { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Doctor> Doctors { get; set; } = null!;
        public virtual DbSet<Graphy> Graphies { get; set; } = null!;
        public virtual DbSet<Idea> Ideas { get; set; } = null!;
        public virtual DbSet<Insurance> Insurances { get; set; } = null!;
        public virtual DbSet<Intro> Intros { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<Reserve> Reserves { get; set; } = null!;
        public virtual DbSet<ReserveStatus> ReserveStatuses { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<Shift> Shifts { get; set; } = null!;
        public virtual DbSet<UserT> UserTs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DarmanYar.mssql.somee.com\\;Database=DarmanYar;user id=DarmanYar_SQLLogin_1;password=endkcy6ilu;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agreement>(entity =>
            {
                entity.ToTable("Agreement");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.Text).HasMaxLength(255);
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(e => new { e.BillId, e.ClinicId, e.Mobile, e.NationalCode });

                entity.ToTable("Bill");

                entity.Property(e => e.BillId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("BillID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.BillPath).HasMaxLength(150);

                entity.Property(e => e.DosierIdReq)
                    .HasMaxLength(50)
                    .HasColumnName("DosierID_Req");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.FromDatePersian)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FromDate_Persian");

                entity.Property(e => e.FullNameReq)
                    .HasMaxLength(200)
                    .HasColumnName("FullName_Req");

                entity.Property(e => e.MobileReq)
                    .HasMaxLength(50)
                    .HasColumnName("Mobile_Req");

                entity.Property(e => e.NationalCodeReq).HasColumnName("NationalCode_Req");

                entity.Property(e => e.RequestDate).HasColumnType("datetime");

                entity.Property(e => e.RequestDatePersian)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RequestDate_Persian");

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.Property(e => e.ToDatePersian)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ToDate_Persian");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.DepartmentName).HasMaxLength(50);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.DrId);

                entity.ToTable("Doctor");

                entity.Property(e => e.ClinikId).HasColumnName("ClinikID");

                entity.Property(e => e.Days).HasMaxLength(150);

                entity.Property(e => e.DrName).HasMaxLength(200);

                entity.Property(e => e.DrSpecialty).HasMaxLength(200);

                entity.Property(e => e.Image).HasMaxLength(150);

                entity.Property(e => e.SectionId).HasColumnName("SectionID");
            });

            modelBuilder.Entity<Graphy>(entity =>
            {
                entity.HasKey(e => new { e.GraphyId, e.ClinicId, e.Mobile, e.NationalCode });

                entity.ToTable("Graphy");

                entity.Property(e => e.GraphyId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("GraphyID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.DosierIdReq)
                    .HasMaxLength(50)
                    .HasColumnName("DosierID_Req");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.FromDatePersian)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FromDate_Persian");

                entity.Property(e => e.FullNameReq)
                    .HasMaxLength(200)
                    .HasColumnName("FullName_Req");

                entity.Property(e => e.GraphyPath).HasMaxLength(150);

                entity.Property(e => e.MobileReq)
                    .HasMaxLength(50)
                    .HasColumnName("Mobile_Req");

                entity.Property(e => e.NationalCodeReq).HasColumnName("NationalCode_Req");

                entity.Property(e => e.RequestDate).HasColumnType("datetime");

                entity.Property(e => e.RequestDatePersian)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RequestDate_Persian");

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.Property(e => e.ToDatePersian)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ToDate_Persian");
            });

            modelBuilder.Entity<Idea>(entity =>
            {
                entity.HasKey(e => new { e.IdeaId, e.ClinicId, e.Mobile, e.NationalCode });

                entity.ToTable("Idea");

                entity.Property(e => e.IdeaId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("IdeaID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.PicFile).HasMaxLength(150);

                entity.Property(e => e.VoiceFile).HasMaxLength(150);
            });

            modelBuilder.Entity<Insurance>(entity =>
            {
                entity.ToTable("Insurance");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.InsuranceName).HasMaxLength(255);
            });

            modelBuilder.Entity<Intro>(entity =>
            {
                entity.HasKey(e => e.ClinikId);

                entity.ToTable("Intro");

                entity.Property(e => e.ClinikId).HasColumnName("ClinikID");

                entity.Property(e => e.MainImage).HasMaxLength(150);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DosierId)
                    .HasMaxLength(50)
                    .HasColumnName("DosierID");

                entity.Property(e => e.InsuranceId).HasColumnName("InsuranceID");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NationalCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(11)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reserve>(entity =>
            {
                entity.HasKey(e => new { e.ReserveId, e.ClinicId, e.Mobile, e.NationalCode });

                entity.ToTable("Reserve");

                entity.Property(e => e.ReserveId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ReserveID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.AgeReq).HasColumnName("Age_Req");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateDatePersian)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CreateDate_Persian");

                entity.Property(e => e.DosierIdReq)
                    .HasMaxLength(50)
                    .HasColumnName("DosierID_Req");

                entity.Property(e => e.FullNameReq)
                    .HasMaxLength(200)
                    .HasColumnName("FullName_Req");

                entity.Property(e => e.GenderReq).HasColumnName("Gender_Req");

                entity.Property(e => e.MobileReq)
                    .HasMaxLength(50)
                    .HasColumnName("Mobile_Req");

                entity.Property(e => e.NationalCodeReq).HasColumnName("NationalCode_Req");

                entity.Property(e => e.ReserveDate).HasColumnType("datetime");

                entity.Property(e => e.ReserveDatePersian)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Reserve_Date_Persian");

                entity.Property(e => e.ShiftId).HasColumnName("ShiftID");

                entity.Property(e => e.Status).HasMaxLength(150);
            });

            modelBuilder.Entity<ReserveStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__ReserveS__C8EE2063A0F2F1FC");

                entity.ToTable("ReserveStatus");

                entity.Property(e => e.StatusName)
                    .HasMaxLength(1)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AI");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.ClinikId).HasColumnName("ClinikID");

                entity.Property(e => e.ServiceImage).HasMaxLength(150);

                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("Shift");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.ShiftName).HasMaxLength(50);
            });

            modelBuilder.Entity<UserT>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("User_t");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.Owner).HasDefaultValueSql("((1))");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(150);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
