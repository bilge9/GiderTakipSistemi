using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiderTakipSistemi.Data;
using GiderTakipSistemi.Models;
using Microsoft.AspNetCore.Authorization;

namespace GiderTakipSistemi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CariKayitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CariKayitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CariKayits
        public async Task<IActionResult> Index()
        {
            return View(await _context.CariKayitlar.ToListAsync());
        }

        // GET: CariKayits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cariKayit = await _context.CariKayitlar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cariKayit == null)
            {
                return NotFound();
            }

            return View(cariKayit);
        }

        // GET: CariKayits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CariKayits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AdSoyad,Telefon,Adres")] CariKayit cariKayit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cariKayit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cariKayit);
        }

        // GET: CariKayits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cariKayit = await _context.CariKayitlar.FindAsync(id);
            if (cariKayit == null)
            {
                return NotFound();
            }
            return View(cariKayit);
        }

        // POST: CariKayits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AdSoyad,Telefon,Adres")] CariKayit cariKayit)
        {
            if (id != cariKayit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cariKayit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CariKayitExists(cariKayit.Id))
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
            return View(cariKayit);
        }

        // GET: CariKayits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cariKayit = await _context.CariKayitlar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cariKayit == null)
            {
                return NotFound();
            }

            return View(cariKayit);
        }

        // POST: CariKayits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cariKayit = await _context.CariKayitlar.FindAsync(id);
            if (cariKayit != null)
            {
                _context.CariKayitlar.Remove(cariKayit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CariKayitExists(int id)
        {
            return _context.CariKayitlar.Any(e => e.Id == id);
        }
    }
}
