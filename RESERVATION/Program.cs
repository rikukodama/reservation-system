using Microsoft.EntityFrameworkCore;
using RESERVATION.Data;
using Stripe;
using System.Configuration;
using System.Net.Http.Headers;
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
    });
builder.Services.AddScoped<RESERVATION.Controllers.SlackService>();
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
