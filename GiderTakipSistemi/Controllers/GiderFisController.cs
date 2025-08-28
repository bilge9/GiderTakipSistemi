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
    [Authorize] // Tüm işlemler giriş yapan kullanıcıya özel
    public class GiderFisController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GiderFisController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: GiderFis
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var giderler = _context.GiderFisleri
                .Where(g => g.UserId == userId)
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem);
            return View(await giderler.ToListAsync());
        }

        // GET: GiderFis/Create
        public IActionResult Create()
        {
            ViewBag.CariList = new SelectList(_context.CariKayitlar, "Id", "AdSoyad");
            ViewBag.KalemList = new SelectList(_context.GiderKalemleri, "Id", "KalemAdi");
            return View();
        }

        // POST: GiderFis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Tarih,Tutar,CariKayitId,GiderKalemId")] GiderFis giderFis)
        {
            if (ModelState.IsValid)
            {
                giderFis.UserId = _userManager.GetUserId(User); // Kullanıcı atanıyor
                _context.Add(giderFis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CariList = new SelectList(_context.CariKayitlar, "Id", "AdSoyad", giderFis.CariKayitId);
            ViewBag.KalemList = new SelectList(_context.GiderKalemleri, "Id", "KalemAdi", giderFis.GiderKalemId);
            return View(giderFis);
        }

        // GET: GiderFis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var giderFis = await _context.GiderFisleri
                .Where(g => g.Id == id && g.UserId == userId)
                .FirstOrDefaultAsync();

            if (giderFis == null) return NotFound();

            ViewBag.CariList = new SelectList(_context.CariKayitlar, "Id", "AdSoyad", giderFis.CariKayitId);
            ViewBag.KalemList = new SelectList(_context.GiderKalemleri, "Id", "KalemAdi", giderFis.GiderKalemId);
            return View(giderFis);
        }

        // POST: GiderFis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tarih,Tutar,CariKayitId,GiderKalemId")] GiderFis giderFis)
        {
            if (id != giderFis.Id) return NotFound();

            var userId = _userManager.GetUserId(User);
            var existingFis = await _context.GiderFisleri
                .Where(g => g.Id == id && g.UserId == userId)
                .FirstOrDefaultAsync();

            if (existingFis == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    existingFis.Tarih = giderFis.Tarih;
                    existingFis.Tutar = giderFis.Tutar;
                    existingFis.CariKayitId = giderFis.CariKayitId;
                    existingFis.GiderKalemId = giderFis.GiderKalemId;

                    _context.Update(existingFis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiderFisExists(id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CariList = new SelectList(_context.CariKayitlar, "Id", "AdSoyad", giderFis.CariKayitId);
            ViewBag.KalemList = new SelectList(_context.GiderKalemleri, "Id", "KalemAdi", giderFis.GiderKalemId);
            return View(giderFis);
        }

        // GET: GiderFis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var giderFis = await _context.GiderFisleri
                .Where(g => g.Id == id && g.UserId == userId)
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem)
                .FirstOrDefaultAsync();

            if (giderFis == null) return NotFound();

            return View(giderFis);
        }

        // POST: GiderFis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var giderFis = await _context.GiderFisleri
                .Where(g => g.Id == id && g.UserId == userId)
                .FirstOrDefaultAsync();

            if (giderFis != null)
            {
                _context.GiderFisleri.Remove(giderFis);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: GiderFis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var giderFis = await _context.GiderFisleri
                .Where(g => g.Id == id && g.UserId == userId)
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem)
                .FirstOrDefaultAsync();

            if (giderFis == null) return NotFound();

            return View(giderFis);
        }

        private bool GiderFisExists(int id)
        {
            return _context.GiderFisleri.Any(e => e.Id == id);
        }
    }
}
