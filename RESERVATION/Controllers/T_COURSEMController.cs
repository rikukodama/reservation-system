using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RESERVATION.Data;
using RESERVATION.Models;

namespace RESERVATION.Controllers
{
    [Authorize(Roles = "admin")]
    public class T_COURSEMController : Controller
    {
        private readonly ReservationContext _context;

        public T_COURSEMController(ReservationContext context)
        {
            _context = context;
        }

        // GET: T_COURSEM
        public async Task<IActionResult> Index()
        {
              return _context.T_COURSEM != null ? 
                          View(await _context.T_COURSEM.ToListAsync()) :
                          Problem("Entity set 'ReservationContext.T_COURSEM'  is null.");
        }

        // GET: T_COURSEM/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.T_COURSEM == null)
            {
                return NotFound();
            }

            var t_COURSEM = await _context.T_COURSEM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (t_COURSEM == null)
            {
                return NotFound();
            }

            return View(t_COURSEM);
        }

        // GET: T_COURSEM/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: T_COURSEM/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,alertMessage")] T_COURSEM t_COURSEM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(t_COURSEM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(t_COURSEM);
        }

        // GET: T_COURSEM/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.T_COURSEM == null)
            {
                return NotFound();
            }

            var t_COURSEM = await _context.T_COURSEM.FindAsync(id);
            if (t_COURSEM == null)
            {
                return NotFound();
            }
            return View(t_COURSEM);
        }

        // POST: T_COURSEM/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,alertMessage")] T_COURSEM t_COURSEM)
        {
            if (id != t_COURSEM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(t_COURSEM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!T_COURSEMExists(t_COURSEM.Id))
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
            return View(t_COURSEM);
        }

        // GET: T_COURSEM/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.T_COURSEM == null)
            {
                return NotFound();
            }

            var t_COURSEM = await _context.T_COURSEM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (t_COURSEM == null)
            {
                return NotFound();
            }

            return View(t_COURSEM);
        }

        // POST: T_COURSEM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.T_COURSEM == null)
            {
                return Problem("Entity set 'ReservationContext.T_COURSEM'  is null.");
            }
            var t_COURSEM = await _context.T_COURSEM.FindAsync(id);
            if (t_COURSEM != null)
            {
                _context.T_COURSEM.Remove(t_COURSEM);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool T_COURSEMExists(int id)
        {
          return (_context.T_COURSEM?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
