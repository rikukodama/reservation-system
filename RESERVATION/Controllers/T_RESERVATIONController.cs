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
    public class T_RESERVATIONController : Controller
    {
        private readonly ReservationContext _context;

        public T_RESERVATIONController(ReservationContext context)
        {
            _context = context;
        }

        // GET: T_RESERVATION
        public async Task<IActionResult> Index()
        {
              return _context.T_RESERVATION != null ? 
                          View(await _context.T_RESERVATION.ToListAsync()) :
                          Problem("Entity set 'ReservationContext.T_RESERVATION'  is null.");
        }

        // GET: T_RESERVATION/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.T_RESERVATION == null)
            {
                return NotFound();
            }

            var t_RESERVATION = await _context.T_RESERVATION
                .FirstOrDefaultAsync(m => m.reservationId == id);
            if (t_RESERVATION == null)
            {
                return NotFound();
            }

            return View(t_RESERVATION);
        }

        // GET: T_RESERVATION/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: T_RESERVATION/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("reservationId,date,time,courceName,optionName,price,alertMessage,update")] T_RESERVATION t_RESERVATION)
        {
            if (ModelState.IsValid)
            {
                _context.Add(t_RESERVATION);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(t_RESERVATION);
        }

        // GET: T_RESERVATION/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.T_RESERVATION == null)
            {
                return NotFound();
            }

            var t_RESERVATION = await _context.T_RESERVATION.FindAsync(id);
            if (t_RESERVATION == null)
            {
                return NotFound();
            }
            return View(t_RESERVATION);
        }

        // POST: T_RESERVATION/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("reservationId,date,time,courceName,optionName,price,alertMessage,update")] T_RESERVATION t_RESERVATION)
        {
            if (id != t_RESERVATION.reservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(t_RESERVATION);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!T_RESERVATIONExists(t_RESERVATION.reservationId))
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
            return View(t_RESERVATION);
        }

        // GET: T_RESERVATION/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.T_RESERVATION == null)
            {
                return NotFound();
            }

            var t_RESERVATION = await _context.T_RESERVATION
                .FirstOrDefaultAsync(m => m.reservationId == id);
            if (t_RESERVATION == null)
            {
                return NotFound();
            }

            return View(t_RESERVATION);
        }

        // POST: T_RESERVATION/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.T_RESERVATION == null)
            {
                return Problem("Entity set 'ReservationContext.T_RESERVATION'  is null.");
            }
            var t_RESERVATION = await _context.T_RESERVATION.FindAsync(id);
            if (t_RESERVATION != null)
            {
                _context.T_RESERVATION.Remove(t_RESERVATION);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool T_RESERVATIONExists(int id)
        {
          return (_context.T_RESERVATION?.Any(e => e.reservationId == id)).GetValueOrDefault();
        }
    }
}
