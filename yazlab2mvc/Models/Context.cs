﻿using Microsoft.EntityFrameworkCore;

namespace yazlab2mvc.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=DESKTOP-PMBMQKM\\SQLEXPRESS; database=yazlab2; integrated security=true;TrustServerCertificate = True;");
            }
        }

        public DbSet<Kullanicilar> Kullanicilar { get; set; }
        public DbSet<Etkinlikler> Etkinlikler { get; set; }
        public DbSet<Katilimcilar> Katilimcilar { get; set; }
        public DbSet<Mesajlar> Mesajlar { get; set; }
        public DbSet<Puanlar> Puanlar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Katilimcilar>()
                .HasKey(k => new { k.KullaniciID, k.EtkinlikID });

            modelBuilder.Entity<Puanlar>()
                .HasKey(p => new { p.KullaniciID, p.KazanilanTarih });

           
            modelBuilder.Entity<Mesajlar>()
                .HasOne(m => m.Gonderici)
                .WithMany(k => k.GonderilenMesajlar)
                .HasForeignKey(m => m.GondericiID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mesajlar>()
                .HasOne(m => m.Alici)
                .WithMany(k => k.AlinanMesajlar)
                .HasForeignKey(m => m.AliciID)
                .OnDelete(DeleteBehavior.Restrict);

            // Etkinlik-Mesajlar ilişkisi
            modelBuilder.Entity<Mesajlar>()
                .HasOne(m => m.Etkinlik)
                .WithMany(e => e.Mesajlar)
                .HasForeignKey(m => m.EtkinlikID);

          

            modelBuilder.Entity<Etkinlikler>()
    .HasOne(e => e.OlusturanKullanici)
    .WithMany(u => u.OlusturduguEtkinlikler)
    .HasForeignKey(e => e.OlusturanKullaniciID)
    .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
