using GiderTakipSistemi.Data;
using GiderTakipSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GiderTakipSistemi.Controllers
{
    [Authorize(Roles = "User")]
    public class KullaniciGiderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public KullaniciGiderController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? cariId, int? giderKalemId)
        {
            var userId = _userManager.GetUserId(User);
            var query = _context.GiderFisleri
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem)
                .Where(g => g.UserId == userId)
                .AsQueryable();

            if (cariId.HasValue)
                query = query.Where(g => g.CariKayitId == cariId.Value);

            if (giderKalemId.HasValue)
                query = query.Where(g => g.GiderKalemId == giderKalemId.Value);

            var giderList = await query.ToListAsync();

            // ✅ Genel toplam
            ViewBag.ToplamGider = giderList.Any() ? giderList.Sum(g => g.Tutar) : 0;

            // ✅ Cari bazlı toplam
            ViewBag.CariBazli = giderList
                .GroupBy(g => g.CariKayit)
                .Select(x => new
                {
                    CariId = x.Key.Id,
                    Cari = x.Key.AdSoyad,
                    ToplamTutar = x.Sum(g => g.Tutar)
                })
                .ToList();

            // ✅ Gider kalemi bazlı toplam (ileride grafik için kullanabilirsin)
            ViewBag.KalemBazli = giderList
                .GroupBy(g => g.GiderKalem)
                .Select(x => new
                {
                    KalemId = x.Key.Id,
                    Kalem = x.Key.KalemAdi,
                    ToplamTutar = x.Sum(g => g.Tutar)
                })
                .ToList();

            return View(giderList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            var gider = await _context.GiderFisleri
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem)
                .FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

            if (gider == null)
                return NotFound();

            return View(gider);
        }
    }
}
