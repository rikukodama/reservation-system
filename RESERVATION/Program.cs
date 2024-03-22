using Microsoft.EntityFrameworkCore;
using RESERVATION.Controllers;
using RESERVATION.Data;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using RESERVATION.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ReservationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("reservationContext")));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ReservationContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
            .AddCookie()
            .AddOpenIdConnect(o =>
            {
                o.ClientId = "2004081012";
                o.ClientSecret = "e3b23055fb8758ed56cf5cf4be13f58e";
                o.ResponseType = OpenIdConnectResponseType.Code;
                o.UseTokenLifetime = true;
                o.SaveTokens = true;
                o.Scope.Add("email");

                o.Configuration = new OpenIdConnectConfiguration
                {
                    Issuer = "https://access.line.me",
                    AuthorizationEndpoint = "https://access.line.me/oauth2/v2.1/authorize?bot_prompt=aggressive",
                    TokenEndpoint = "https://api.line.me/oauth2/v2.1/token"
                };
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(o.ClientSecret)),
                    NameClaimType = "name",
                    ValidAudience = o.ClientId
                };
            });
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
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
