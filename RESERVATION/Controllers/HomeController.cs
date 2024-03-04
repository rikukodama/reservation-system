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
            ViewData["coursemList"] = await _context.T_COURSEM.ToListAsync();
            
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Course([FromBody] DateViewModel model)
        {
            ViewData["res_date"] = model.res_date;
            ViewData["coursem_id"] = model.coursem_id;
            ViewData["courseList"] = await _context.T_COURSE.ToListAsync();
            ViewData["optionList"] = await _context.T_OPTION.ToListAsync();

            return View();
        }
        public async Task<IActionResult> Reservation()
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
