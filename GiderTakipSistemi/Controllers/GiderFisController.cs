using GiderTakipSistemi.Data;
using GiderTakipSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GiderTakipSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GiderFisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GiderFisController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Admin paneli: Tüm giderler
        public async Task<IActionResult> Index()
        {
            var giderler = await _context.GiderFisleri
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem)
                .Include(g => g.User)
                .ToListAsync();

            return View(giderler);
        }

        // Admin: Yeni gider ekleme
        public IActionResult Create()
        {
            ViewBag.UserList = new SelectList(_userManager.Users, "Id", "Email");
            ViewBag.CariList = new SelectList(_context.CariKayitlar, "Id", "AdSoyad");
            ViewBag.KalemList = new SelectList(_context.GiderKalemleri, "Id", "KalemAdi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Tarih,Tutar,CariKayitId,GiderKalemId")] GiderFis giderFis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giderFis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.UserList = new SelectList(_userManager.Users, "Id", "Email", giderFis.UserId);
            ViewBag.CariList = new SelectList(_context.CariKayitlar, "Id", "AdSoyad", giderFis.CariKayitId);
            ViewBag.KalemList = new SelectList(_context.GiderKalemleri, "Id", "KalemAdi", giderFis.GiderKalemId);
            return View(giderFis);
        }

        // Admin: Düzenle
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var giderFis = await _context.GiderFisleri.FindAsync(id);
            if (giderFis == null) return NotFound();

            ViewBag.UserList = new SelectList(_userManager.Users, "Id", "Email", giderFis.UserId);
            ViewBag.CariList = new SelectList(_context.CariKayitlar, "Id", "AdSoyad", giderFis.CariKayitId);
            ViewBag.KalemList = new SelectList(_context.GiderKalemleri, "Id", "KalemAdi", giderFis.GiderKalemId);
            return View(giderFis);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Tarih,Tutar,CariKayitId,GiderKalemId")] GiderFis giderFis)
        {
            if (id != giderFis.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giderFis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.GiderFisleri.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.UserList = new SelectList(_userManager.Users, "Id", "Email", giderFis.UserId);
            ViewBag.CariList = new SelectList(_context.CariKayitlar, "Id", "AdSoyad", giderFis.CariKayitId);
            ViewBag.KalemList = new SelectList(_context.GiderKalemleri, "Id", "KalemAdi", giderFis.GiderKalemId);
            return View(giderFis);
        }

        // Admin: Sil
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var giderFis = await _context.GiderFisleri
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (giderFis == null) return NotFound();

            return View(giderFis);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var giderFis = await _context.GiderFisleri.FindAsync(id);
            if (giderFis != null)
            {
                _context.GiderFisleri.Remove(giderFis);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Admin: Detaylar
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var giderFis = await _context.GiderFisleri
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (giderFis == null) return NotFound();

            return View(giderFis);
        }
    }
}
