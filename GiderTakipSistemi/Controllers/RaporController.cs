using GiderTakipSistemi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GiderTakipSistemi.Controllers
{
    public class RaporController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaporController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime? baslangic, DateTime? bitis, int? cariId, int? kalemId)
        {
            var query = _context.GiderFisleri
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem)
                .AsQueryable();

            // Tarih filtresi
            if (baslangic.HasValue)
                query = query.Where(g => g.Tarih >= baslangic.Value);

            if (bitis.HasValue)
                query = query.Where(g => g.Tarih <= bitis.Value);

            // Cari filtresi
            if (cariId.HasValue)
                query = query.Where(g => g.CariKayitId == cariId.Value);

            // Kalem filtresi
            if (kalemId.HasValue)
                query = query.Where(g => g.GiderKalemId == kalemId.Value);

            // Genel toplam
            var toplamGider = query.Sum(g => g.Tutar);

            // Cari bazlı toplam
            var cariBazli = query
                .GroupBy(g => g.CariKayit.AdSoyad)
                .Select(x => new
                {
                    Cari = x.Key,
                    ToplamTutar = x.Sum(g => g.Tutar)
                }).ToList();

            // Kalem bazlı toplam
            var kalemBazli = query
                .GroupBy(g => g.GiderKalem.KalemAdi)
                .Select(x => new
                {
                    Kalem = x.Key,
                    ToplamTutar = x.Sum(g => g.Tutar)
                }).ToList();

            // Dropdown listeleri
            ViewBag.CariList = new SelectList(_context.CariKayitlar, "Id", "AdSoyad", cariId);
            ViewBag.KalemList = new SelectList(_context.GiderKalemleri, "Id", "KalemAdi", kalemId);

            ViewBag.ToplamGider = toplamGider;
            ViewBag.CariBazli = cariBazli;
            ViewBag.KalemBazli = kalemBazli;

            // Tarihleri tekrar gönder
            ViewBag.Baslangic = baslangic?.ToString("yyyy-MM-dd");
            ViewBag.Bitis = bitis?.ToString("yyyy-MM-dd");

            return View();
        }
    }
    }