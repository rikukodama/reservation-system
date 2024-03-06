using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESERVATION.Data;
using RESERVATION.Models;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Stripe;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Stripe.Checkout;



namespace RESERVATION.Controllers
{
    public class HomeController : Controller
    {

        private readonly ReservationContext _context;

        public HomeController(ILogger<HomeController> logger, ReservationContext context, IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration.GetSection("Stripe:ApiKey").Value;
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
        public IActionResult Verify(string stripeToken, decimal amount)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(string stripeToken, decimal amount)
        {
            StripeConfiguration.ApiKey = "sk_test_51OrCH42L9BZ0orkJ7t4xNcwqisT5LyBPfEfA81j3DltMcmrzXddT2evOAWhDJN4o8SQ74kiqiytCvAZF54VgablH00hN1jCfVT";
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = 10000,
                Currency = "usd",
                // In the latest version of the API, specifying the `automatic_payment_methods` parameter is optional because Stripe enables its functionality by default.
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });
            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Course([Bind("res_date,coursem_id")] DateViewModel model)
        {
            ViewData["res_date"] = model.res_date;
            ViewData["coursem_id"] = model.coursem_id;
            ViewData["courseList"] = await _context.T_COURSE.ToListAsync();
            ViewData["optionList"] = await _context.T_OPTION.ToListAsync();

            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservation([Bind("res_date,coursem_id,course_id,option_id,price")] OptionViewModel model)
        {
            ViewData["res_date"] = model.res_date;
            ViewData["coursem_id"] = model.coursem_id;
            ViewData["course_id"] = model.course_id;
            ViewData["option_id"] = model.option_id;
            ViewData["price"] = model.price;
            if (model.course_id == null || _context.T_COURSEM == null)
            {
                return NotFound();
            }

            var t_COURSEM = await _context.T_COURSEM.FindAsync(model.coursem_id);
            if (t_COURSEM == null)
            {
                return NotFound();
            }
            ViewData["coursem_name"] = t_COURSEM.Name;
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
