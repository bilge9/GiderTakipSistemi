using GiderTakipSistemi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiderTakipSistemi.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CariKayit> CariKayitlar { get; set; }
        public DbSet<GiderFis> GiderFisleri { get; set; }
        public DbSet<GiderKalem> GiderKalemleri { get; set; }

        //Tutarlar çok büyük veya hassas olursa veri kaybını önlemek için
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//Primary key yok hatası oluşmaması için.
            modelBuilder.Entity<GiderFis>()
                .Property(g => g.Tutar)
                .HasPrecision(18, 2);  // precision: toplam basamak, scale: virgülden sonra basamak
        }


    }

}

