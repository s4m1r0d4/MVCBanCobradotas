using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BanCobradotas.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

[Route("[controller]")]
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
        var created = await service.RequestCuentaUsuario(model);

        if (created is null || created.IDUsuario > 0)
            return RedirectToAction("IniciarSesion", "Inicio");

        ViewData["Mensaje"] = "Error al crear la solicitud de usuario";

        return View();
    }

    public IActionResult IniciarSesion()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> IniciarSesion(string usuario, string contrasena)
    {
        var usr = await service.GetCuentaIngreso(usuario, contrasena);

        if (usr == null) {
            ViewData["Mensaje"] = "Error iniciando sesion";
            return View();
        }



        /* 
        List<Claim> claims = new() {
            new Claim(ClaimTypes.)
        }
        https://www.youtube.com/watch?v=x0MBbx7lym8&t=1850s
        */

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }
}