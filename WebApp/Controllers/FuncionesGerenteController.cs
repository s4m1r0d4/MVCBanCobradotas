using System.Security.Claims;
using BanCobradotas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

[Authorize(Policy = "GerenteOnly")]
public class FuncionesGerenteController : Controller
{
    private readonly BanCobradotasContext db;

    public FuncionesGerenteController(BanCobradotasContext injectedContext)
    {
        db = injectedContext;
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


    public async Task<IActionResult> AdministrarCuentas()
    {
        // TODO: Implement this
    
        AdministrarCuentasModel model = new()
        {
            EmpleadoAlta = new(),
            UsuariosAlta = new()
        };
        return View(model);
    }

    public async Task<IActionResult> CrearEmpleado()
    {
        // TODO: Implement this
        return View();
    }

    public async Task<IActionResult> Reportes()
    {
        // TODO: Implement this
        return View();
    }

    public async Task<IActionResult> Vacaciones()
    {
        // TODO: Implement this
        return View();
    }


}