using Microsoft.EntityFrameworkCore;
using RESERVATION.Controllers;
using RESERVATION.Data;
using Stripe;
using System.Configuration;
using System.Net.Http.Headers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ReservationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("reservationContext")));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("SlackApiClient")
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        return handler;
    })
    .ConfigureHttpClient((serviceProvider, client) =>
    {
        client.BaseAddress = new Uri("https://slack.com/api/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "xoxb-6478304789431-6759776124819-BuwoiWbv6ELJIvcYSf1NNmlI");
    });

builder.Services.AddScoped<RESERVATION.Controllers.SlackService>();
builder.Services.AddScoped<ReservationService>();
var app = builder.Build();
//builder.Services.AddDbContext<ReservationContext>(options =>
//            options.UseSqlServer(builder.Configuration.GetConnectionString("reservationContext")));



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
