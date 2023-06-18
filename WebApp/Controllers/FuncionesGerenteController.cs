using System.Security.Claims;
using BanCobradotas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

[Authorize(Policy = "GerenteOnly")]
public class FuncionesGerenteController : Controller
{
    private readonly IFuncionesGerenteService service;

    public FuncionesGerenteController(IFuncionesGerenteService injectedService)
    {
        service = injectedService;
    }

    // Que DRY ni que nada
    public string GetNombre()
    {
        return HttpContext.User.Claims!.Where(c => c.Type == ClaimTypes.Name)
            .Select(c => c.Value)
            .SingleOrDefault() ?? "Unknown";
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Nombre"] = GetNombre();
        return View();
    }


}