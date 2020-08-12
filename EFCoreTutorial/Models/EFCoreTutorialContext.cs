using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreTutorial.Models
{
    public partial class EFCoreTutorialContext : DbContext
    {
        public EFCoreTutorialContext()
        {
        }

        public EFCoreTutorialContext(DbContextOptions<EFCoreTutorialContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Musteri> Musteri { get; set; }
        public virtual DbSet<Sepet> Sepet { get; set; }
        public virtual DbSet<SepetUrun> SepetUrun { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Musteri>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.Sehir).HasMaxLength(50);

                entity.Property(e => e.Soyad).HasMaxLength(50);
            });

            modelBuilder.Entity<Sepet>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MusteriId).HasColumnName("MusteriID");

                entity.Property(e => e.Tarih).HasColumnType("datetime");
            });

            modelBuilder.Entity<SepetUrun>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Aciklama).HasMaxLength(50);

                entity.Property(e => e.SepetId).HasColumnName("SepetID");
            });
        }
    }
}
