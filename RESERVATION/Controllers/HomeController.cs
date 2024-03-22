using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESERVATION.Data;
using RESERVATION.Models;
using System.Diagnostics;
using Stripe;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Data.SqlClient;
using Slack.Webhooks.Blocks;
using Google.Apis.Calendar.v3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNet.Security.OAuth.Line;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Com.CloudRail.SI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Line.Messaging;
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
    public class ReservationService
    {

        public async Task<string> CreateReservationEvent(DateTime reservationDate, string reservationStatus)
        {
     try
     {
         // Path to your service account key JSON file
         string serviceAccountKeyPath = "./Secrets/credentials.json";

     // User email associated with Google Calendar
     string userEmail = "devflex0401@gmail.com";

     // Load and create credentials
     using (var stream = new FileStream(serviceAccountKeyPath, FileMode.Open, FileAccess.Read))
     {
         var credential = GoogleCredential.FromStream(stream)
             .CreateScoped(CalendarService.Scope.CalendarEvents);

         var initializer = new BaseClientService.Initializer()
         {
             HttpClientInitializer = credential,
             ApplicationName = "sauna reservation",
         };

         // Instantiate the Calendar
         var calendarService = new CalendarService(initializer);

         // Create new event
         var newEvent = new Google.Apis.Calendar.v3.Data.Event
         {
             Summary = "Reservation",
             Start = new EventDateTime()
             {
                 DateTime = reservationDate,
                 TimeZone = "Asia/Tokyo"
             },
             End = new EventDateTime()
             {
                 DateTime = reservationDate.AddHours(1),
                 TimeZone = "Asia/Tokyo"
             },
             Description = $"Status: {reservationStatus}"
         };

         // Set calendar being shared with Service Account.

         var calendarId = userEmail;
         var request = calendarService.Events.Insert(newEvent, calendarId);

                    // Execute request and retrieve the created event data                      
                    var createdEvent = await request.ExecuteAsync();

                    // Get the eventId of the created event
                    string eventId = createdEvent.Id;

                    Console.WriteLine("Event created successfully. Event ID: " + eventId);

                    return eventId;
         }
     }
     catch (Exception ex)
     {
                Console.WriteLine("Error creating event: " + ex.Message);
                return null;
     }
 

 }
        public async Task DeleteReservationEvent(string eventId)
        {
            try
            {
                // Path to your service account key JSON file
                string serviceAccountKeyPath = "./Secrets/credentials.json";

                // User email associated with Google Calendar
                string userEmail = "devflex0401@gmail.com";

                // Load and create credentials
                using (var stream = new FileStream(serviceAccountKeyPath, FileMode.Open, FileAccess.Read))
                {
                    var credential = GoogleCredential.FromStream(stream)
                        .CreateScoped(CalendarService.Scope.CalendarEvents);

                    var initializer = new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "sauna reservation",
                    };

                    // Instantiate the Calendar
                    var calendarService = new CalendarService(initializer);

                    // Set calendar being shared with Service Account
                    var calendarId = userEmail;

                    // Delete the event
                    await calendarService.Events.Delete(calendarId, eventId).ExecuteAsync();

                    Console.WriteLine("Event deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting event: " + ex.Message);
            }
        }
    }
    public class LineSettings
    {
        public string LoginClientId { get; set; }
        public string LoginClientSecret { get; set; }
        public string MessagingAccessToken { get; set; }
    }
  
    public class HomeController : Controller
    {

        private readonly ReservationContext _context;
        private readonly SlackService _slackService;
        private readonly ReservationService _reservationService;
        private readonly IConfiguration _configuration;
        private LineSettings lineSettings;
        public HomeController(ILogger<HomeController> logger, ReservationContext context, IConfiguration configuration, SlackService slackService, ReservationService reservationService, IOptions<LineSettings> options)
        {
            StripeConfiguration.ApiKey = configuration.GetSection("Stripe:ApiKey").Value;
            _context = context;
            _slackService = slackService;
            _reservationService = reservationService;
            _configuration = configuration;
            lineSettings = options.Value;

            string channelId = _configuration["LineLogin:ChannelId"];
            string channelSecret = _configuration["LineLogin:ChannelSecret"];
        }
     
        public async Task<IActionResult> Index()
        {
            ViewData["coursemList"] = await _context.T_COURSEM.ToListAsync();
            ViewData["reservation"] = await _context.T_RESERVATION.ToListAsync();
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PushMessage(string pushMessage)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            using (var line = new Line.Messaging.LineMessagingClient(lineSettings.MessagingAccessToken))
            {
                await line.PushMessageAsync(userId, pushMessage);
            }
            return View();
        }

        public ActionResult GetReservation([FromBody] DateTime resDate)
        {
            var status = _context.T_RESERVATION
                .Count(r => r.date == resDate);
            var count = _context.T_COURSEM.ToList().Count();
            bool reservationStatus = status == count;
            return Json(reservationStatus);

        }
        public class MyModel
        {
            public DateTime Date { get; set; }
            public int Number { get; set; }
        }
        public ActionResult GetStatus([FromBody] MyModel model)
        {
            var status = _context.T_RESERVATION
                .Count(r => r.date == model.Date && r.coursem_id == model.Number);
            bool reservationStatus = status == 1;
            return Json(reservationStatus);

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
                Amount = (long)(price),  // Stripe expects price in cents, so multiply by 100
                Currency = "jpy",
                // In the latest version of the API, specifying the `automatic_payment_methods` parameter is optional because Stripe enables its functionality by default.
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });
            Debug.WriteLine("paymentIntent : " + paymentIntent.ClientSecret);

            return Json(new { clientSecret = paymentIntent.ClientSecret });

        }

        [HttpPost]
        public IActionResult Refund([FromBody] int Id)
        {

            var reservationToRemove = _context.T_RESERVATION.FirstOrDefault(r => r.reservationId == Id);
            var refundService = new RefundService();
            var refundOptions = new RefundCreateOptions
            {
                PaymentIntent = reservationToRemove.paymentIntentid
            };

            var refund = refundService.Create(refundOptions);

            // Handle the refund response as needed
            if (refund.Status == "succeeded")
            {
                // Refund was successful
                var t_COURSEM = _context.T_COURSEM.FirstOrDefault(r => r.Id == reservationToRemove.coursem_id);

                var channel = "D06NQG697R7";
                _context.T_RESERVATION.Remove(reservationToRemove);
                _context.SaveChanges();
                var time = (DateTime)reservationToRemove.date;
                var message = time.Year + "/" + time.Month + "/" + time.Day + " " + t_COURSEM.Name + " 予約はキャンセルされました。";
                _slackService.SendMessage(channel, message);
                _reservationService.DeleteReservationEvent(reservationToRemove.calendarid);
                return Ok("ok"); 
            }
            else
            {
                // Refund failed
                // Add any additional error handling here
                return BadRequest("Refund failed.");
            }
        }
        // Example usage in your project code
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Privacy([Bind("reservationId,date,coursem_id,cource_id,option_id,price,username,phonenumber,mail,update,paymentIntentid,calendarid")] T_RESERVATION t_RESERVATION)
        {
            if (ModelState.IsValid)
            {
                var Date = DateTime.Now;
                var channel = "D06NQG697R7";
                //      var name = await _context.T_COURSEM.ToListAsync();
                var t_COURSEM = await _context.T_COURSEM.FindAsync(t_RESERVATION.coursem_id);
                var time = (DateTime)t_RESERVATION.date;
                var message = time.Year + "/" + time.Month + "/" + time.Day + " " + t_COURSEM.Name + " 予約されました。";
                var eventid = await _reservationService.CreateReservationEvent(time, message);
                t_RESERVATION.calendarid = eventid;
                _context.Add(t_RESERVATION);
                await _context.SaveChangesAsync();
                var result = await _slackService.SendMessage(channel, message);

                return RedirectToAction(nameof(Privacy));
            }

            return View(t_RESERVATION);
        }
        public async Task<IActionResult> Privacy()
        {
            ViewData["reservation"] = await _context.T_RESERVATION.ToListAsync();

            return View();
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
