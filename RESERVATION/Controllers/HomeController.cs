using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESERVATION.Data;
using RESERVATION.Models;
using System.Diagnostics;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace RESERVATION.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly ILogger<HomeController> _logger;
        private readonly ReservationContext _context;

        public HomeController(ILogger<HomeController> logger, ReservationContext context)
        {
            _logger = logger;
            _context = context;
        }

      
        public async Task<IActionResult> Index()
        {
            return _context.T_COURSEM != null ?
                        View(await _context.T_COURSEM.ToListAsync()) :
                        Problem("Entity set 'ReservationContext.T_COURSE'  is null.");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Course()
        {
            return _context.T_COURSE != null ?
                        View(await _context.T_COURSE.ToListAsync()) :
                        Problem("Entity set 'ReservationContext.T_OPTION'  is null.");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
