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

        ViewBag.CariBazli = giderler
            .GroupBy(g => g.CariKayit.AdSoyad)
            .Select(gr => new { Cari = gr.Key, ToplamTutar = gr.Sum(g => g.Tutar) })
            .ToList();

        ViewBag.KalemBazli = giderler
            .GroupBy(g => g.GiderKalem.KalemAdi)
            .Select(gr => new { Kalem = gr.Key, ToplamTutar = gr.Sum(g => g.Tutar) })
            .ToList();

        return View();
    }
}
