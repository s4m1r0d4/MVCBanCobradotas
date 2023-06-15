using BanCobradotas.Models;
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
}