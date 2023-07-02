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

        public virtual DbSet<Bill> Bills { get; set; } = null!;
        public virtual DbSet<Doctor> Doctors { get; set; } = null!;
        public virtual DbSet<Graphy> Graphies { get; set; } = null!;
        public virtual DbSet<Intro> Intros { get; set; } = null!;
        public virtual DbSet<Reserve> Reserves { get; set; } = null!;
        public virtual DbSet<ReserveStatus> ReserveStatuses { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
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
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(e => new { e.BillId, e.ClinicId, e.Mobile, e.NationalCode })
                    .HasName("Bill_PK");

                entity.ToTable("Bill");

                entity.Property(e => e.BillId).HasColumnName("BillID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BillPath).HasMaxLength(200);

                entity.Property(e => e.DosierIdReq)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DosierID_Req");

                entity.Property(e => e.FromDateReq)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FromDate_Req");

                entity.Property(e => e.FullNameReq)
                    .HasMaxLength(200)
                    .HasColumnName("FullName_Req");

                entity.Property(e => e.MobileReq)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Mobile_Req");

                entity.Property(e => e.NationalCodeReq).HasColumnName("NationalCode_Req");

                entity.Property(e => e.RequestDate)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ToDateReq)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ToDate_Req");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.DrId)
                    .HasName("PK__Doctor__0150EEDB0BE0C78A");

                entity.ToTable("Doctor");

                entity.Property(e => e.DrId).HasColumnName("DrID");

                entity.Property(e => e.ClinikId).HasColumnName("ClinikID");

                entity.Property(e => e.Days)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DrName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DrSpecialty)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SectionId).HasColumnName("SectionID");
            });

            modelBuilder.Entity<Graphy>(entity =>
            {
                entity.HasKey(e => new { e.GraphyId, e.ClinicId, e.Mobile, e.NationalCode })
                    .HasName("Graphy_PK");

                entity.ToTable("Graphy");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DosierIdReq)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DosierID_Req");

                entity.Property(e => e.FromDate)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullNameReq)
                    .HasMaxLength(200)
                    .HasColumnName("FullName_Req");

                entity.Property(e => e.GraphyPath).HasMaxLength(200);

                entity.Property(e => e.MobileReq)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Mobile_Req");

                entity.Property(e => e.NationalCodeReq).HasColumnName("NationalCode_Req");

                entity.Property(e => e.RequestDate)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ToDate)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Intro>(entity =>
            {
                entity.HasKey(e => e.ClinikId)
                    .HasName("PK__Intro__2DB5809C8C93C54B");

                entity.ToTable("Intro");

                entity.Property(e => e.ClinikId)
                    .ValueGeneratedNever()
                    .HasColumnName("ClinikID");

                entity.Property(e => e.Description)
                    .HasMaxLength(4000)
                    .UseCollation("Persian_100_CI_AS");

                entity.Property(e => e.MainImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reserve>(entity =>
            {
                entity.HasKey(e => new { e.ReserveId, e.ClinicId, e.Mobile, e.NationalCode })
                    .HasName("Reserve_PK");

                entity.ToTable("Reserve");

                entity.Property(e => e.ReserveId).HasColumnName("ReserveID");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AgeReq).HasColumnName("Age_Req");

                entity.Property(e => e.CreateDate)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DosierIdReq)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DosierID_Req");

                entity.Property(e => e.FullNameReq)
                    .HasMaxLength(200)
                    .HasColumnName("FullName_Req");

                entity.Property(e => e.GenderReq).HasColumnName("Gender_Req");

                entity.Property(e => e.MobileReq)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Mobile_Req");

                entity.Property(e => e.NationalCodeReq).HasColumnName("NationalCode_Req");

                entity.Property(e => e.ReserveDate)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Reserve_Date");

                entity.Property(e => e.Status).HasMaxLength(100);
            });

            modelBuilder.Entity<ReserveStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK__ReserveS__36257A1878327516");

                entity.ToTable("ReserveStatus");

                entity.Property(e => e.StatusId).HasColumnName("statusId");

                entity.Property(e => e.ClinicId).HasColumnName("ClinicID");

                entity.Property(e => e.StatusName).HasMaxLength(100);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.ClinikId).HasColumnName("ClinikID");

                entity.Property(e => e.Description)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ServiceImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserT>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__User_t__1788CCACA2D1FB32");

                entity.ToTable("User_t");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Owner).HasDefaultValueSql("((1))");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
