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

    public async Task<IActionResult> Solicitudes()
    {
        var solicitudes = await db.Prestamos.Where(p => p.FechaAprobacion == null).ToListAsync();
        var model = new HistorialPrestamosModel()
        {
            Prestamos = solicitudes
        };
        return View(model);
    }


    // Get
    public async Task<IActionResult> Calcular(long IDPrestamo)
    {
        // RQNF12: El cálculo de un préstamo no puede ser mayor al 50% del máximo del total que se encuentra en la cuenta.
        // Only allow Gerente to accept 
        string role = HttpContext.User.Claims!.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault()!;
        if (role == "Empleado") {

        }
        throw new NotImplementedException();
    }

}