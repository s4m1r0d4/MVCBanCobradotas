using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BanCobradotas.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

// Políticas de autorización, ejemplo:
// [Authorize(Policy = "GerenteOnly")]
// [Authorize(Policy = "EmpleadoOnly")]
// [Authorize(Policy = "CuentaBancoOnly")]
// [Authorize] -> Así nomás iniciando sesión
// [AllowAnonymous] -> Permite ingresar al controlador sin iniciar sesión
public class TestController : Controller
{
    // Agarrar el servicio del controlador para acceder la base de datos
    private readonly ITestService service;

    // Constructor donde agarra el servicio inyectado
    public TestController(ITestService injectedService)
    {
        service = injectedService;
    }

    public async Task<IActionResult> Index()
    {
        Prestamo p = new(); // Un prestamo todo inventado nomás por motivos de ejemplo

        // Mandarle a la vista la lista de boletos

        List<Boleto>? boletos = await service.GetBoletos(p);

        // Manera 1, solo debe usarse para cantidades pequeñas de información, no listas >:(
        ViewData["Boletos"] = boletos;

        // Manera 2, poquitos más pasos pero permite mandar mayores cantidades de información de manera correcta
        // crear un fokin modelo
        TestBoletosModel model = new()
        {
            Boletos = boletos
        };


        // Return de Manera 1
        // return View();

        // Return de Manera 2
        return View(model);
    }
}