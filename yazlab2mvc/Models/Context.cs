using Microsoft.EntityFrameworkCore;

namespace yazlab2mvc.Models
{
    public class Context : DbContext
    {
        // Constructor (Yapıcı) method
        public Context(DbContextOptions<Context> options) : base(options) { }

        // DbSet Properties
        public DbSet<Kullanicilar> Kullanicilar { get; set; }
        public DbSet<Etkinlikler> Etkinlikler { get; set; }
        public DbSet<Katilimcilar> Katilimcilar { get; set; }
        public DbSet<Mesajlar> Mesajlar { get; set; }
        public DbSet<Puanlar> Puanlar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite Keys (Birleşik Anahtarlar)
            modelBuilder.Entity<Katilimcilar>()
                .HasKey(k => new { k.KullaniciID, k.EtkinlikID });

            modelBuilder.Entity<Puanlar>()
                .HasKey(p => new { p.KullaniciID, p.KazanilanTarih });

            // Relationships and Constraints
            // Mesajlar ve Kullanıcılar (Gönderici ve Alıcı) arasında ilişkiler
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

            // Mesajlar ve Etkinlikler arasında ilişki
            modelBuilder.Entity<Mesajlar>()
                .HasOne(m => m.Etkinlik)
                .WithMany(e => e.Mesajlar)
                .HasForeignKey(m => m.EtkinlikID);
        }
    }
}
