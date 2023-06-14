using BanCobradotas.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Services.Implementations;
using WebApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BanCobradotasContext>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("SQLite3"));
    }
);

builder.Services.AddScoped<ICuentaIngresoService, CuentaIngresoService>();
// TODO: add the rest of the services

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/LogIn/IniciarSesion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    }
);

builder.Services.AddAuthorization(options =>
{
    /* options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build(); */
    options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("IDNomina"));
    options.AddPolicy("GerenteOnly", policy => policy.RequireClaim("IDGerente"));
    options.AddPolicy("CuentaBancoOnly", policy => policy.RequireClaim("IDCuentaBancaria"));
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
            new ResponseCacheAttribute
            {
                NoStore = true,
                Location = ResponseCacheLocation.None,
            }
        );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
