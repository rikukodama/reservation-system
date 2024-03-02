using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RESERVATION.Data;
using RESERVATION.Models;

namespace RESERVATION.Controllers
{
    public class OptionController : Controller
    {
        private readonly ReservationContext _context;

        public OptionController(ReservationContext context)
        {
            _context = context;
        }

        // GET: Option
        public async Task<IActionResult> Index()
        {
              return _context.T_OPTION != null ? 
                          View(await _context.T_OPTION.ToListAsync()) :
                          Problem("Entity set 'ReservationContext.T_OPTION'  is null.");
        }

        // GET: Option/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.T_OPTION == null)
            {
                return NotFound();
            }

            var t_OPTION = await _context.T_OPTION
                .FirstOrDefaultAsync(m => m.OptionId == id);
            if (t_OPTION == null)
            {
                return NotFound();
            }

            return View(t_OPTION);
        }

        // GET: Option/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Option/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OptionId,courceId,optionName,price,alertMessage")] T_OPTION t_OPTION)
        {
            if (ModelState.IsValid)
            {
                _context.Add(t_OPTION);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(t_OPTION);
        }

        // GET: Option/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.T_OPTION == null)
            {
                return NotFound();
            }

            var t_OPTION = await _context.T_OPTION.FindAsync(id);
            if (t_OPTION == null)
            {
                return NotFound();
            }
            return View(t_OPTION);
        }

        // POST: Option/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OptionId,courceId,optionName,price,alertMessage")] T_OPTION t_OPTION)
        {
            if (id != t_OPTION.OptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(t_OPTION);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!T_OPTIONExists(t_OPTION.OptionId))
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
            return View(t_OPTION);
        }

        // GET: Option/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.T_OPTION == null)
            {
                return NotFound();
            }

            var t_OPTION = await _context.T_OPTION
                .FirstOrDefaultAsync(m => m.OptionId == id);
            if (t_OPTION == null)
            {
                return NotFound();
            }

            return View(t_OPTION);
        }

        // POST: Option/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.T_OPTION == null)
            {
                return Problem("Entity set 'ReservationContext.T_OPTION'  is null.");
            }
            var t_OPTION = await _context.T_OPTION.FindAsync(id);
            if (t_OPTION != null)
            {
                _context.T_OPTION.Remove(t_OPTION);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool T_OPTIONExists(int id)
        {
          return (_context.T_OPTION?.Any(e => e.OptionId == id)).GetValueOrDefault();
        }
    }
}
