using GiderTakipSistemi.Models;
using System.Linq;

namespace GiderTakipSistemi.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Cari kayıt ekleme
            if (!context.CariKayitlar.Any())
            {
                context.CariKayitlar.AddRange(
                    new CariKayit { AdSoyad = "Ahmet Yılmaz", Telefon = "05551234567", Adres = "İstanbul" },
                    new CariKayit { AdSoyad = "Ayşe Demir", Telefon = "05559876543", Adres = "Ankara" },
                    new CariKayit { AdSoyad = "Mehmet Kaya", Telefon = "05557654321", Adres = "İzmir" }
                );
            }

            // Gider kalemi ekleme
            if (!context.GiderKalemleri.Any())
            {
                context.GiderKalemleri.AddRange(
                    new GiderKalem { KalemAdi = "Kira" },
                    new GiderKalem { KalemAdi = "Elektrik" },
                    new GiderKalem { KalemAdi = "Su" },
                    new GiderKalem { KalemAdi = "İnternet" }
                );
            }

            context.SaveChanges();
        }
    }
}
