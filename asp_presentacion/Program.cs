using Microsoft.AspNetCore.Authentication.Cookies;

using asp_presentacion;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/ventanas/IniciarSesion";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        option.AccessDeniedPath = "/Pages/Index";
    });


var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder, builder.Services);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

startup.Configure(app, app.Environment);
app.Run();