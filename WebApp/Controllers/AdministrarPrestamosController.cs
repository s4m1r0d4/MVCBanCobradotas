using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using BanCobradotas.Models;
using BanCobradotas.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

[Authorize(Policy = "EmployeeOnly")]
public class AdministrarPrestamosController : Controller
{
    private readonly BanCobradotasContext db;

    public AdministrarPrestamosController(BanCobradotasContext injectedContext)
    {
        db = injectedContext;
    }

    public async Task<IActionResult> Index()
    {
        var solicitudes = await db.Prestamos.Where(p => p.FechaAprobacion == null).ToListAsync();

        // RQNF28: El tiempo de validación de los préstamos es de 48Hrs
        var twoDays = new TimeSpan(2, 0, 0, 0);
        var solicitudesCaducas = solicitudes.Where(s => DateTime.Now - s.FechaSolicitud > twoDays);
        if (solicitudesCaducas.Any()) {
            db.Prestamos.RemoveRange(solicitudesCaducas);
            int affected = await db.SaveChangesAsync();
            if (affected == 0)
                return Problem("Error eliminando las solicitudes caducas");
        }

        // RQNF27: Los préstamos deben de durar en el archivo hasta 6 meses después de la eliminación del usuario que tenía asignado dicho(s) préstamo(s).
        var seisMeses = new TimeSpan(60, 0, 0, 0);
        var prestamosDeUsuarioEliminado = await db.Prestamos
            .Where(p => (p.IDCuentaBancaria == null))
            .ToListAsync();

        var prestamosCaducos = prestamosDeUsuarioEliminado.Where(p => DateTime.Now - p.FechaAprobacion > seisMeses);

        if (prestamosCaducos.Any()) {
            db.Prestamos.RemoveRange(prestamosCaducos);
            int affected = await db.SaveChangesAsync();
            if (affected == 0)
                return Problem("Error eliminando los préstamos de usuarios eliminados");
        }

        var model = new HistorialPrestamosModel()
        {
            Prestamos = solicitudes
        };
        if (TempData.ContainsKey("Msg")) {
            ViewData["Msg"] = TempData["Msg"];
        }
        return View(model);
    }


    // Get AdministrarPresamos/2
    public async Task<IActionResult> Calcular(long? id)
    {
        if (id is null)
            return View();

        var prestamo = await db.Prestamos
                        .Include(p => p.IDCuentaBancariaNavigation)
                        .ThenInclude(cuentab => cuentab.Gerente)
                        .Include(p => p.IDCuentaBancariaNavigation)
                        .ThenInclude(cuentab => cuentab.Usuario)
                        .FirstOrDefaultAsync(p => p.IDPrestamo == id);
        if (prestamo is null)
            return Problem("Prestamo no encontrado");

        // Calcular la tasa de interés
        double interes;
        if (prestamo.IDCuentaBancariaNavigation.Gerente is not null) {
            // Préstamo solicitado por gerente
            // RQNF24: El préstamo del gerente tiene un interés de solo 10.2%
            interes = 0.102D;
        } else {
            // Préstamo solicitado por cliente
            // RQNF13: El cálculo de la tasa de interés es basado en el número de meses que se escoge, si es 6 meses es 12%, si es 12 meses es 18%, si es 24 meses es 27.9% y para el caso de 36 meses es 42%
            interes = prestamo.NumMeses switch
            {
                6 => 0.12D,
                12 => 0.18D,
                24 => 0.279D,
                36 => 0.42D
            };
        }

        double pagoTotal = prestamo.Cantidad * (interes + 1);
        ViewData["pagoTotal"] = pagoTotal;
        if (pagoTotal > prestamo.IDCuentaBancariaNavigation.Saldo * 0.5D) {
            // RQNF12: El cálculo de un préstamo no puede ser mayor al 50% del máximo del total que se encuentra en la cuenta.
            ViewData["Err"] = $"Error, el pago total supera al 50% del saldo de la cuenta. {pagoTotal} > {prestamo.IDCuentaBancariaNavigation.Saldo * 0.5D}";
        }

        // RQF12: El empleado podrá revisar el estatus del último préstamo conocido por el usuario y hasta los 10 últimos préstamos del usuario.
        var ultimosPrestamos = await db.Prestamos
                                        .Where(p =>
                                            p.IDCuentaBancaria == prestamo.IDCuentaBancaria
                                            && p.FechaAprobacion != null)
                                        .OrderByDescending(p => p.FechaAprobacion)
                                        .Take(10)
                                        .ToListAsync();
        if (ultimosPrestamos is not null) {
            ViewData["ultimosPrestamos"] = ultimosPrestamos;
        }

        double pagoMensual = pagoTotal / prestamo.NumMeses;
        prestamo.Interes = interes * 100;
        prestamo.PagoMensual = pagoMensual;

        int affected = await db.SaveChangesAsync();
        if (affected != 1) {
            TempData["Msg"] = "Error calculando el préstamo";
            return RedirectToAction(nameof(Index));
        }

        TempData["Msg"] = "Préstamo calculado exitósamente";

        return View(prestamo);
    }

    [HttpPost, ActionName("Aceptar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AceptarPrestamo(long? id)
    {
        if (id is null)
            return RedirectToAction("Index");

        var prestamo = await db.Prestamos.FirstOrDefaultAsync(p => p.IDPrestamo == id);
        if (prestamo is null) {
            TempData["Msg"] = "Prestamo no encontrado";
            return RedirectToAction(nameof(Index));
        }

        // RQF10: El empleado podrá aceptar los préstamos de 6 y 12 meses.
        if (HttpContext.User.IsInRole("Empleado") && prestamo.NumMeses > 12) {
            TempData["Msg"] = "El empleado solo puede aceptar los préstamos de 6 y 12 meses";
            return RedirectToAction(nameof(Index));
        }

        string? nominaStr = HttpContext.User.Claims.Where(c => c.Type == "IDNomina").Select(c => c.Value).FirstOrDefault();
        long nomina = long.Parse(nominaStr);
        prestamo.IDNomina = nomina;
        prestamo.FechaAprobacion = DateTime.Now;
        // RQNF26: La fecha de aprobación no puede ser antes de la fecha de solicitud.
        if (prestamo.FechaAprobacion < prestamo.FechaSolicitud) {
            TempData["Msg"] = "Error, la fecha de aprobación no puede ser antes de la fecha de solicitud";
            return RedirectToAction(nameof(Index));
        }

        prestamo.IDEstado = 2; // Aceptado

        int affected = await db.SaveChangesAsync();
        if (affected != 1) {
            TempData["Msg"] = "Error aceptando el préstamo";
            return RedirectToAction(nameof(Index));
        }

        TempData["Msg"] = "Préstamo aceptado exitósamente";
        return RedirectToAction(nameof(Index));
    }
}