using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiderTakipSistemi.Data;
using GiderTakipSistemi.Models;

namespace GiderTakipSistemi.Controllers
{
    public class GiderFisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GiderFisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GiderFis
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GiderFisleri.Include(g => g.CariKayit).Include(g => g.GiderKalem);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GiderFis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giderFis = await _context.GiderFisleri
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giderFis == null)
            {
                return NotFound();
            }

            return View(giderFis);
        }

        // GET: GiderFis/Create
        public IActionResult Create()
        {
            ViewData["CariKayitId"] = new SelectList(_context.CariKayitlar, "Id", "Id");
            ViewData["GiderKalemId"] = new SelectList(_context.GiderKalemleri, "Id", "Id");
            return View();
        }

        // POST: GiderFis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GiderKalemId,CariKayitId,Tutar,Tarih")] GiderFis giderFis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giderFis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CariKayitId"] = new SelectList(_context.CariKayitlar, "Id", "Id", giderFis.CariKayitId);
            ViewData["GiderKalemId"] = new SelectList(_context.GiderKalemleri, "Id", "Id", giderFis.GiderKalemId);
            return View(giderFis);
        }

        // GET: GiderFis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giderFis = await _context.GiderFisleri.FindAsync(id);
            if (giderFis == null)
            {
                return NotFound();
            }
            ViewData["CariKayitId"] = new SelectList(_context.CariKayitlar, "Id", "Id", giderFis.CariKayitId);
            ViewData["GiderKalemId"] = new SelectList(_context.GiderKalemleri, "Id", "Id", giderFis.GiderKalemId);
            return View(giderFis);
        }

        // POST: GiderFis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GiderKalemId,CariKayitId,Tutar,Tarih")] GiderFis giderFis)
        {
            if (id != giderFis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giderFis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiderFisExists(giderFis.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CariKayitId"] = new SelectList(_context.CariKayitlar, "Id", "Id", giderFis.CariKayitId);
            ViewData["GiderKalemId"] = new SelectList(_context.GiderKalemleri, "Id", "Id", giderFis.GiderKalemId);
            return View(giderFis);
        }

        // GET: GiderFis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giderFis = await _context.GiderFisleri
                .Include(g => g.CariKayit)
                .Include(g => g.GiderKalem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giderFis == null)
            {
                return NotFound();
            }

            return View(giderFis);
        }

        // POST: GiderFis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var giderFis = await _context.GiderFisleri.FindAsync(id);
            if (giderFis != null)
            {
                _context.GiderFisleri.Remove(giderFis);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiderFisExists(int id)
        {
            return _context.GiderFisleri.Any(e => e.Id == id);
        }
    }
}
