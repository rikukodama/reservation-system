using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESERVATION.Data;
using RESERVATION.Models;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Stripe;
using SlackAPI;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using RESERVATION.Migrations;

using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection.Metadata;


namespace RESERVATION.Controllers
{
    public class SlackService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SlackService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Method to send a message to Slack
        public async Task<bool> SendMessage(string channel, string message)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("SlackApiClient");

                var parameters = new Dictionary<string, string>
    {
    { "channel", channel },
    { "text", message }
};

                var content = new FormUrlEncodedContent(parameters);

                var response = await client.PostAsync("chat.postMessage", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("Response: " + responseContent);
                    return true;
                }
                else
                {
                    // Log the response or handle the error accordingly

                    return false;
                }
            }
            catch (HttpRequestException ex)
            {

                if (ex.InnerException != null)
                    Debug.WriteLine("Inner Exception: " + ex.InnerException);

                // Access additional error details
                if (ex.StatusCode.HasValue)
                {
                    Debug.WriteLine("Status Code: " + ex.StatusCode);
                }

                // Optionally, add more specific error handling or rethrow the exception
                throw;
            }
        }
    }
    public class HomeController : Controller
    {

        private readonly ReservationContext _context;
        private readonly SlackService _slackService;
        public HomeController(ILogger<HomeController> logger, ReservationContext context, IConfiguration configuration, SlackService slackService)
        {
            StripeConfiguration.ApiKey = configuration.GetSection("Stripe:ApiKey").Value;
            _context = context;
            _slackService = slackService;
        }
     
        public async Task<IActionResult> Index()
        {
            ViewData["coursemList"] = await _context.T_COURSEM.ToListAsync();

            return View();
        }

        public IActionResult Verify()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify([Bind("reservationId,date,coursem_id,cource_id,option_id,price,username,phonenumber,mail,update")] T_RESERVATION model)
        {
            ViewData["reservationId"] = model.reservationId;
            ViewData["date"] = model.date;
            ViewData["coursem_id"] = model.coursem_id;
            ViewData["cource_id"] = model.cource_id;
            ViewData["option_id"] = model.option_id;
            ViewData["price"] = model.price;
            ViewData["username"] = model.username;
            ViewData["phonenumber"] = model.phonenumber;
            ViewData["mail"] = model.mail;
            ViewData["update"] = model.update;

            return View();
        }

        // POST: T_RESERVATION/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Checkout([FromBody] decimal price)
        {

            //    StripeConfiguration.ApiKey = "sk_test_51OrCH42L9BZ0orkJ7t4xNcwqisT5LyBPfEfA81j3DltMcmrzXddT2evOAWhDJN4o8SQ74kiqiytCvAZF54VgablH00hN1jCfVT";
            var paymentIntentService = new PaymentIntentService();

            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = (long)(price * 100),  // Stripe expects price in cents, so multiply by 100
                Currency = "jpy",
                // In the latest version of the API, specifying the `automatic_payment_methods` parameter is optional because Stripe enables its functionality by default.
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });

            return Json(new { clientSecret = paymentIntent.ClientSecret });

        }
        [HttpGet]
        public async Task<IActionResult> Privacy()
        {
            var channel = "D06NQG697R7";
            var message = "hello";
            var result = await _slackService.SendMessage(channel, message);
            if (result)
            {
                // Slack message was sent successfully
                
                return View();
            }
            else
            {
                // Slack message sending failed
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Refund(string paymentIntentId)
        {
            StripeConfiguration.ApiKey = "sk_test_51OrCH42L9BZ0orkJ7t4xNcwqisT5LyBPfEfA81j3DltMcmrzXddT2evOAWhDJN4o8SQ74kiqiytCvAZF54VgablH00hN1jCfVT";

            var refundService = new RefundService();
            var refundOptions = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId
            };
            var refund = refundService.Create(refundOptions);

            return Json(new { success = true, refundId = refund.Id });
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
            Debug.WriteLine("Price value: " + model.price);

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
