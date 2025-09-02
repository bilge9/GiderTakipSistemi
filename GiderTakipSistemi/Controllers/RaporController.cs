using GiderTakipSistemi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class RaporController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public RaporController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(DateTime? baslangic, DateTime? bitis, int? cariId, int? kalemId)
    {
        var userId = _userManager.GetUserId(User);
        var query = _context.GiderFisleri
            .Where(g => g.UserId == userId)
            .Include(g => g.CariKayit)
            .Include(g => g.GiderKalem)
            .AsQueryable();

        if (baslangic.HasValue)
            query = query.Where(g => g.Tarih >= baslangic.Value);
        if (bitis.HasValue)
            query = query.Where(g => g.Tarih <= bitis.Value);
        if (cariId.HasValue)
            query = query.Where(g => g.CariKayitId == cariId.Value);
        if (kalemId.HasValue)
            query = query.Where(g => g.GiderKalemId == kalemId.Value);

        var giderler = await query.ToListAsync();

        ViewBag.TumGiderler = giderler;

        ViewBag.Baslangic = baslangic?.ToString("yyyy-MM-dd");
        ViewBag.Bitis = bitis?.ToString("yyyy-MM-dd");
        ViewBag.CariId = cariId;
        ViewBag.KalemId = kalemId;

        ViewBag.CariList = _context.CariKayitlar.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.AdSoyad
        }).ToList();

        ViewBag.KalemList = _context.GiderKalemleri.Select(k => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Value = k.Id.ToString(),
            Text = k.KalemAdi
        }).ToList();

        ViewBag.ToplamGider = giderler.Sum(g => g.Tutar);

        // Cari bazlı toplam
        ViewBag.CariBazli = _context.GiderFisleri
            .Where(g => g.UserId == userId) // isteğe göre filtre
            .GroupBy(g => new { g.CariKayit.Id, g.CariKayit.AdSoyad })
            .Select(g => new
            {
                CariId = g.Key.Id,
                Cari = g.Key.AdSoyad,
                ToplamTutar = g.Sum(x => x.Tutar)
            })
            .ToList();

        // Gider kalemi bazlı toplam
        ViewBag.KalemBazli = _context.GiderFisleri
            .Where(g => g.UserId == userId)
            .GroupBy(g => new { g.GiderKalem.Id, g.GiderKalem.KalemAdi })
            .Select(g => new
            {
                KalemId = g.Key.Id,
                Kalem = g.Key.KalemAdi,
                ToplamTutar = g.Sum(x => x.Tutar)
            })
            .ToList();
        // Özet için
    var kalemBazli = giderler
        .GroupBy(g => g.GiderKalem.KalemAdi)
        .Select(g => new
        {
            Kalem = g.Key,
            Toplam = g.Sum(x => x.Tutar)
        })
        .ToList();

        ViewBag.GiderKalemleri = kalemBazli.Select(k => k.Kalem).ToArray();
        ViewBag.GiderToplamlari = kalemBazli.Select(k => k.Toplam).ToArray();


        return View();
    }
}
