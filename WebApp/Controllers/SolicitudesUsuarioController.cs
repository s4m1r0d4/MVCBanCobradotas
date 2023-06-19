using System.Security.Claims;
using BanCobradotas.Models;
using BanCobradotas.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace WebApp.Controllers;

[Authorize(Policy = "GerenteOnly")]
public class SolicitudesUsuarioController : Controller
{
    private readonly BanCobradotasContext db;

    public SolicitudesUsuarioController(BanCobradotasContext injectedContext)
    {
        db = injectedContext;
    }

    public async Task<IActionResult> Index(long? id)
    {
        var solicitudes = await db.Usuarios.Where(u => u.IDEstado == 1).ToListAsync();

        var model = new SolicitudesUsuarioModel()
        {
            SolicitudesUsuario = solicitudes
        };

        if (id is not null) {
            ViewData["Usuario"] = id;
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("Error!");
    }

    [HttpPost]
    public async Task<IActionResult> Rechazar(long? id)
    {
        if (id is null)
            return RedirectToAction(nameof(Index));

        Usuario? usr = await db.Usuarios.FirstOrDefaultAsync(u => u.IDUsuario == id);

        if (usr is null)
            return Problem("Usuario no encontrado");

        db.Usuarios.Remove(usr);
        await db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Aceptar(long? id)
    {
        if (id is null)
            return RedirectToAction(nameof(Index));

        Usuario? usr = await db.Usuarios.FirstOrDefaultAsync(u => u.IDUsuario == id);
        if (usr is null)
            return Problem("Usuario no encontrado");

        // Create CuentaBancaria
        CuentaBancaria cb = new()
        {
            Saldo = 10_000D
        };
        db.CuentasBancaria.Add(cb);
        int affected = await db.SaveChangesAsync();
        if (affected == 0)
            return Problem("Error creando CuentaBancaria");

        // Create CuentaIngreso
        (affected, long cuentaIngresoID) = db.AddCuentaIngreso(usr.Nombre, usr.Apellido, usr.FechaNacimiento);
        if (affected == 0)
            return Problem("Error creando CuentaIngreso");

        // Ingresar el id del gerente que la aceptó
        ClaimsPrincipal claimuser = HttpContext.User;
        string? idnomina_str = claimuser.Claims!.Where(c => c.Type == "IDNomina")
            .Select(c => c.Value).SingleOrDefault();
        if (idnomina_str is null)
            return Problem("Error al consultar la nomina");

        usr.IDCuentaBancaria = cb.IDCuentaBancaria;
        usr.IDCuentaIngreso = cuentaIngresoID;
        usr.IDNomina = long.Parse(idnomina_str);
        usr.IDEstado = 2; // En espera

        affected = await db.SaveChangesAsync();
        if (affected == 0)
            return Problem("Error actualizando al usuario");


        return RedirectToAction(nameof(Index));
    }
}