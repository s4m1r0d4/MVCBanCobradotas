using System.Security.Claims;
using BanCobradotas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

[Authorize(Policy = "CuentaBancoOnly")]
public class PrestamosController : Controller
{
    private readonly BanCobradotasContext db;

    public PrestamosController(BanCobradotasContext injectedContext)
    {
        db = injectedContext;
    }
    //verificar que el saldo en la cuenta sea 10,000MXN o mas.

    [HttpPost]
    public async Task<IActionResult> SolicitarPrestamo(SolicitarPrestamoModel model)
    {

        return View();
    }

}