using Microsoft.EntityFrameworkCore;
using RESERVATION.Data;
using System.Configuration;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ReservationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("reservationContext")));
// Add services to the container.
builder.Services.AddControllersWithViews();

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
