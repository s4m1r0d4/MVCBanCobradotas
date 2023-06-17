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

    public IActionResult IniciarSesion()
    {
        return View();
    }


}