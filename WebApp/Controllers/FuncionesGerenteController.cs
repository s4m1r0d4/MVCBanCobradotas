using System.Security.Claims;
using BanCobradotas.Models;
using BanCobradotas.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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


    public async Task<IActionResult> AdministrarCuentas(long? id, long? id2)
    {
        // TODO: Implement this
        var empleados = await db.Empleados.Where(e => e.IDEmpleado == 1).ToListAsync();

        var model = new AdministrarCuentasModel()
        {
            EmpleadoAlta = empleados
        };

        if (id is not null) {
            ViewData["Empleado"] = id;
        }

        return View(model);

        var usuarios = await db.Usuarios.Where(u => u.IDUsuario == 1).ToListAsync();

        var model2 = new AdministrarCuentasModel()
        {
            UsuarioAlta = usuarios
        };

        if (id2 is not null) {
            ViewData["Usuario"] = id2;
        }

        return View(model2);

    }

    [HttpPost]
    public async Task<IActionResult> eliminar(long? id)
    {
        if (id is null) {
            return RedirectToAction(nameof(AdministrarCuentas));
        }

        Empleado? emp = await db.Empleados.FirstOrDefaultAsync(e => e.IDEmpleado == id);

        db.Empleados.Remove(emp);
        await db.SaveChangesAsync();

        return RedirectToAction(nameof(AdministrarCuentas));

    }

    [HttpPost]
    public async Task<IActionResult> eliminarUsr(long? id)
    {
        // El Gerente sólo podrá dar de baja,a aquellos empleados 
        //y usuarios que no tengan registros de ningún préstamos en los últimos 6 meses.
        if (id is null) {
            return RedirectToAction(nameof(AdministrarCuentas));
        }

        Usuario? usr = await db.Usuarios.FirstOrDefaultAsync(u => u.IDUsuario == id);

        db.Usuarios.Remove(usr);
        await db.SaveChangesAsync();

        return RedirectToAction(nameof(AdministrarCuentas));
    }

    [HttpPost]
    public async Task<IActionResult> CrearEmpleado(Empleado model)
    {
        //RQF8: El empleado será solo generado por el Gerente.
        // TODO: Implement this
        db.Empleados.Add(model);
        int affected = await db.SaveChangesAsync();

        ViewData["Msg"] = "Empleado Creado con Exito";

        return View();
    }

    public async Task<IActionResult> Reportes()
    {
        // TODO: Implement this
        return View();
    }

    public async Task<IActionResult> Vacaciones(DiaVacacion model)
    {
        //RQNF18: El gerente puede usar solo 4 días seguidos de vacaciones.
        // TODO: Implement this
        //hacer que si son mas de 4 dias los que eligion en date salga un text warning
        db.DiasVacacion.Add(model);
        int affected = await db.SaveChangesAsync();

        ViewData["Msg"] = "Dias de vacaciones admitidos";

        return View();
    }
}