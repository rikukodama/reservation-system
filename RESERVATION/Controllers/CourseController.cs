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
    public class CourseController : Controller
    {
        private readonly ReservationContext _context;

        public CourseController(ReservationContext context)
        {
            _context = context;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
              return _context.T_COURSE != null ? 
                          View(await _context.T_COURSE.ToListAsync()) :
                          Problem("Entity set 'ReservationContext.T_COURSE'  is null.");
        }

        // GET: Course/Details/5
        
        // GET: Course/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("courceId,courceName,limitMinNum,limitMaxNum,price,alertMessage")] T_COURSE t_COURSE)
        {
            if (ModelState.IsValid)
            {
                _context.Add(t_COURSE);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(t_COURSE);
        }
        // GET: Option/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.T_COURSE == null)
            {
                return NotFound();
            }

            var t_COURSE = await _context.T_COURSE
                .FirstOrDefaultAsync(m => m.courceId == id);
            if (t_COURSE == null)
            {
                return NotFound();
            }

            return View(t_COURSE);
        }
        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.T_COURSE == null)
            {
                return NotFound();
            }

            var t_COURSE = await _context.T_COURSE.FindAsync(id);
            if (t_COURSE == null)
            {
                return NotFound();
            }
            return View(t_COURSE);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("courceId,courceName,limitMinNum,limitMaxNum,price,alertMessage")] T_COURSE t_COURSE)
        {
            if (id != t_COURSE.courceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(t_COURSE);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!T_COURSEExists(t_COURSE.courceId))
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
            return View(t_COURSE);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.T_COURSE == null)
            {
                return NotFound();
            }

            var t_COURSE = await _context.T_COURSE
                .FirstOrDefaultAsync(m => m.courceId == id);
            if (t_COURSE == null)
            {
                return NotFound();
            }

            return View(t_COURSE);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.T_COURSE == null)
            {
                return Problem("Entity set 'ReservationContext.T_COURSE'  is null.");
            }
            var t_COURSE = await _context.T_COURSE.FindAsync(id);
            if (t_COURSE != null)
            {
                _context.T_COURSE.Remove(t_COURSE);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private bool T_COURSEExists(int id)
        {
          return (_context.T_COURSE?.Any(e => e.courceId == id)).GetValueOrDefault();
        }
    }
}
