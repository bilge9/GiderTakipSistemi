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
    public class GiderKalemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GiderKalemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GiderKalems
        public async Task<IActionResult> Index()
        {
            return View(await _context.GiderKalemleri.ToListAsync());
        }

        // GET: GiderKalems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giderKalem = await _context.GiderKalemleri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giderKalem == null)
            {
                return NotFound();
            }

            return View(giderKalem);
        }

        // GET: GiderKalems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GiderKalems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KalemAdi")] GiderKalem giderKalem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(giderKalem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(giderKalem);
        }

        // GET: GiderKalems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giderKalem = await _context.GiderKalemleri.FindAsync(id);
            if (giderKalem == null)
            {
                return NotFound();
            }
            return View(giderKalem);
        }

        // POST: GiderKalems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KalemAdi")] GiderKalem giderKalem)
        {
            if (id != giderKalem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(giderKalem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiderKalemExists(giderKalem.Id))
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
            return View(giderKalem);
        }

        // GET: GiderKalems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var giderKalem = await _context.GiderKalemleri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (giderKalem == null)
            {
                return NotFound();
            }

            return View(giderKalem);
        }

        // POST: GiderKalems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var giderKalem = await _context.GiderKalemleri.FindAsync(id);
            if (giderKalem != null)
            {
                _context.GiderKalemleri.Remove(giderKalem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiderKalemExists(int id)
        {
            return _context.GiderKalemleri.Any(e => e.Id == id);
        }
    }
}
