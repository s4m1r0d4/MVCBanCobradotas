using System.Security.Claims;
using BanCobradotas.Models;
using BanCobradotas.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

[Authorize(Policy = "CuentaBancoOnly")]
public class CuentaBancariaController : Controller
{
    private readonly BanCobradotasContext db;

    public CuentaBancariaController(BanCobradotasContext injectedContext)
    {
        db = injectedContext;
    }

    public long GetIDCuentaBancaria()
    {
        ClaimsPrincipal claimsPrincipal = HttpContext.User;

        long IDCuentaBancaria = long.Parse(claimsPrincipal.Claims!.Where(c => c.Type == "IDCuentaBancaria")
            .Select(c => c.Value)
            .SingleOrDefault()!);

        return IDCuentaBancaria;
    }

    public string GetNombre()
    {
        return HttpContext.User.Claims!.Where(c => c.Type == ClaimTypes.Name)
            .Select(c => c.Value)
            .SingleOrDefault() ?? "Unknown";
    }

    public async Task<IActionResult> Index()
    {
        long cuentaID = GetIDCuentaBancaria();
        var cuenta = await db.CuentasBancaria.FirstOrDefaultAsync(c => c.IDCuentaBancaria == cuentaID);
        if (cuenta is null)
            return Problem("Error! La cuenta bancaria no existe");

        ViewData["Saldo"] = cuenta.Saldo;
        ViewData["NumCuenta"] = cuenta.IDCuentaBancaria;
        ViewData["Nombre"] = GetNombre();

        var prestamo = await db.GetPrestamoActivo(cuenta);
        if (prestamo == null)
            return View();

        return View(prestamo);
    }

    //verificar que el saldo en la cuenta sea 10,000MXN o mas.
    [HttpPost]
    public async Task<IActionResult> SolicitarPrestamo(SolicitarPrestamoModel model)
    {
        return View();
    }

    public async Task<IActionResult> HistorialPrestamos()
    {
        long cuentaID = GetIDCuentaBancaria();
        var cuenta = await db.CuentasBancaria.FirstOrDefaultAsync(c => c.IDCuentaBancaria == cuentaID);
        if (cuenta is null)
            return Problem("Error! La cuenta bancaria no existe");

        var prestamos = await db.Prestamos.Where(p => p.IDCuentaBancaria == cuentaID)
                            .Include(p => p.Pagos).ToListAsync();

        HistorialPrestamosModel model = new()
        {
            Prestamos = prestamos
        };

        return View(model);
    }

    public async Task<IActionResult> HistorialPagos()
    {
        throw new NotImplementedException();
    }
}