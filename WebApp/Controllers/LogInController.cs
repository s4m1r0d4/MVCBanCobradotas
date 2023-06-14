using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BanCobradotas.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

// [Route("[controller]")]
// ^^^^^^^^^^^^^^^^^^^^ Pinche línea del demonio nos quitó 5 días
[AllowAnonymous]
public class LogInController : Controller
{
    private readonly ICuentaIngresoService service;

    public LogInController(ICuentaIngresoService injectedService)
    {
        service = injectedService;
    }

    public IActionResult Registrarse()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registrarse(Usuario model)
    {
        var (_, err) = await service.RequestCuentaUsuario(model);

        ViewData["Mensaje"] = err;

        return View();
    }

    public IActionResult IniciarSesion()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> IniciarSesion(string usuario, string contrasena)
    {
        var (usr, err) = await service.GetCuentaIngreso(usuario, contrasena);

        if (usr == null) {
            ViewData["Mensaje"] = err;
            return View();
        }

        List<Claim> claims = new();

        string nombre = "Unknown";
        if (usr.Empleado is not null) {
            nombre = usr.Empleado.Nombre;
            claims.Add(new Claim(ClaimTypes.Role, "Empleado"));
            claims.Add(new Claim("IDNomina", usr.Empleado.IDNomina.ToString()));
            claims.Add(new Claim("IDEmpleado", usr.Empleado.IDEmpleado.ToString()));
        }

        if (usr.Gerente is not null) {
            nombre = usr.Gerente.Nombre;
            claims.Add(new Claim(ClaimTypes.Role, "Gerente"));
            claims.Add(new Claim("IDNomina", usr.Gerente.IDNomina.ToString()));
            claims.Add(new Claim("IDGerente", usr.Gerente.IDGerente.ToString()));
            claims.Add(new Claim("IDCuentaBancaria", usr.Gerente.IDCuentaBancaria.ToString()));
        }

        if (usr.UsuarioNavigation is not null) {
            nombre = usr.UsuarioNavigation.Nombre;
            claims.Add(new Claim(ClaimTypes.Role, "Usuario"));
            claims.Add(new Claim("IDUsuario", usr.UsuarioNavigation.IDUsuario.ToString()));
            claims.Add(new Claim("IDCuentaBancaria", usr.UsuarioNavigation.IDCuentaBancaria.ToString()!));
        }

        claims.Add(new Claim(ClaimTypes.Name, nombre));

        ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties properties = new()
        {
            AllowRefresh = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            properties
        );

        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}