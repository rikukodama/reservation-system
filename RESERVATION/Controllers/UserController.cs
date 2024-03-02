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
    public class UserController : Controller
    {
        private readonly ReservationContext _context;

        public UserController(ReservationContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
              return _context.T_USER != null ? 
                          View(await _context.T_USER.ToListAsync()) :
                          Problem("Entity set 'ReservationContext.T_USER'  is null.");
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.T_USER == null)
            {
                return NotFound();
            }

            var t_USER = await _context.T_USER
                .FirstOrDefaultAsync(m => m.Id == id);
            if (t_USER == null)
            {
                return NotFound();
            }

            return View(t_USER);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Contact,Birthday,Gender,History")] T_USER t_USER)
        {
            if (ModelState.IsValid)
            {
                _context.Add(t_USER);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(t_USER);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.T_USER == null)
            {
                return NotFound();
            }

            var t_USER = await _context.T_USER.FindAsync(id);
            if (t_USER == null)
            {
                return NotFound();
            }
            return View(t_USER);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Contact,Birthday,Gender,History")] T_USER t_USER)
        {
            if (id != t_USER.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(t_USER);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!T_USERExists(t_USER.Id))
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
            return View(t_USER);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.T_USER == null)
            {
                return NotFound();
            }

            var t_USER = await _context.T_USER
                .FirstOrDefaultAsync(m => m.Id == id);
            if (t_USER == null)
            {
                return NotFound();
            }

            return View(t_USER);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.T_USER == null)
            {
                return Problem("Entity set 'ReservationContext.T_USER'  is null.");
            }
            var t_USER = await _context.T_USER.FindAsync(id);
            if (t_USER != null)
            {
                _context.T_USER.Remove(t_USER);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool T_USERExists(int id)
        {
          return (_context.T_USER?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
