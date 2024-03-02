using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESERVATION.Data;
using RESERVATION.Models;
using System.Diagnostics;

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

        public IActionResult Index()
        {
            return _context.T_COURSE != null ?
                          View(await _context.T_COURSE.ToListAsync()) :
                          Problem("Entity set 'ReservationContext.T_COURSE'  is null.");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
