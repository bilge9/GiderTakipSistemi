using GiderTakipSistemi.Data;
using GiderTakipSistemi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GiderTakipSistemi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GiderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public GiderController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // Tüm kullanıcıların giderlerini listele
        public async Task<IActionResult> Index()
        {
            var giderler = _context.GiderFisleri
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem)
                .Include(g => g.User); // Kullanıcı bilgisi
            return View(await giderler.ToListAsync());
        }

        // Gider ekle (Admin istediği kullanıcı için)
        public IActionResult Create()
        {
            ViewBag.CariList = new SelectList(_context.CariKayitlar, "Id", "AdSoyad");
            ViewBag.KalemList = new SelectList(_context.GiderKalemleri, "Id", "KalemAdi");
            ViewBag.UserList = new SelectList(_userManager.Users, "Id", "Email"); // Kullanıcı seçimi
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Tarih,Tutar,CariKayitId,GiderKalemId,UserId")] GiderFis giderFis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giderFis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CariList = new SelectList(_context.CariKayitlar, "Id", "AdSoyad", giderFis.CariKayitId);
            ViewBag.KalemList = new SelectList(_context.GiderKalemleri, "Id", "KalemAdi", giderFis.GiderKalemId);
            ViewBag.UserList = new SelectList(_userManager.Users, "Id", "Email", giderFis.UserId);
            return View(giderFis);
        }
    }
}
