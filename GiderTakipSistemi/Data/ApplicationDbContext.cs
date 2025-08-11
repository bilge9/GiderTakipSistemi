using Microsoft.EntityFrameworkCore;
using GiderTakipSistemi.Models;

namespace GiderTakipSistemi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CariKayit> CariKayitlar { get; set; }
        public DbSet<GiderFis> GiderFisleri { get; set; }
        public DbSet<GiderKalem> GiderKalemleri { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }

        //Tutarlar çok büyük veya hassas olursa veri kaybını önlemek için
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GiderFis>()
                .Property(g => g.Tutar)
                .HasPrecision(18, 2);  // precision: toplam basamak, scale: virgülden sonra basamak
        }


    }

}

