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

    public long GetIDPrestamo()
    {
        ClaimsPrincipal claimsPrincipal = HttpContext.User;

        long IDPrestamo = long.Parse(claimsPrincipal.Claims!.Where(c => c.Type == "IDPrestamo")
            .Select(c => c.Value)
            .SingleOrDefault()!);

        return IDPrestamo;
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

        var prestamo = db.GetPrestamoActivo(cuenta);
        if (prestamo == null)
            return View();

        return View(prestamo);
    }

    // GET
    public async Task<IActionResult> SolicitarPrestamo()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SolicitarPrestamo(Prestamo model)
    {
        /* 
            RQNF14: El empleado sólo podrá aprobar préstamos si es que no se tiene otro préstamo activo.
            RQNF15: La excepción en el cuál podría aprobar un segundo préstamo “activo” es si y sólo si se encuentra en el último mes del préstamo vigente.

            ¿Por qué te dejaría solicitar préstamos si es que tienes uno activo?
        */

        long cuentaID = GetIDCuentaBancaria();
        var cuenta = await db.CuentasBancaria.Include(c => c.Prestamos).FirstOrDefaultAsync(c => c.IDCuentaBancaria == cuentaID);
        if (cuenta is null)
            return Problem("Error! La cuenta bancaria no existe");

        var currentDate = DateTime.Now;
        var prestamosActivos = cuenta.Prestamos.Where(p => p.FechaLiquidacion == null);
        if (prestamosActivos is not null && prestamosActivos.Any()) {
            string err = "Ya tienes un préstamo activo";
            if (prestamosActivos.Count() > 1) {
                ViewData["Msg"] = err;
                return View();
            }


            Prestamo prestamoActivo = prestamosActivos.First()!;
            DateTime fechaAprobacion = (DateTime)prestamoActivo.FechaAprobacion!;
            var calculatedFechaLiquidacion = new DateTime(fechaAprobacion.Year, fechaAprobacion.Month, fechaAprobacion.Day);
            calculatedFechaLiquidacion = calculatedFechaLiquidacion.AddMonths((int)prestamoActivo.NumMeses);

            if (!((currentDate.Year == calculatedFechaLiquidacion.Year)
                && (currentDate.Month == calculatedFechaLiquidacion.Month))) {
                ViewData["Msg"] = err;
                return View();
            }
        }

        long[] validMonths = { 6, 12, 24, 36 };

        if (!validMonths.Contains(model.NumMeses)) {
            ViewData["Msg"] = "El número de meses solo puede ser 6, 12, 24 o 36";
            return View();
        }

        model.FechaSolicitud = currentDate;
        model.IDEstado = 1;
        model.IDCuentaBancaria = cuentaID;

        db.Prestamos.Add(model);
        int affected = await db.SaveChangesAsync();

        if (affected != 1) {
            ViewData["Msg"] = "Error creando el préstamo";
        }

        ViewData["Msg"] = "Préstamo solicitado exitosamente";

        return View();
    }

    public async Task<IActionResult> HistorialPrestamos()
    {
        long cuentaID = GetIDCuentaBancaria();
        var prestamos = await db.Prestamos.Where(p => p.IDCuentaBancaria == cuentaID && p.FechaAprobacion != null)
                            .Include(p => p.Pagos).ToListAsync();

        HistorialPrestamosModel model = new()
        {
            Prestamos = prestamos
        };

        return View(model);
    }

    public async Task<IActionResult> HistorialPagos()
    {
        long IDPagos = GetIDPrestamo();
        var pagos = await db.Pagos.Where(p => p.IDPrestamo == IDPagos).ToListAsync();
        HistorialPrestamosModel model = new()
        {
            Pagos = pagos
        };
        return View(model);
    }
}