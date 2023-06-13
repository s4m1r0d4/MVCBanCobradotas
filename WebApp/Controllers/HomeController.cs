using System.Diagnostics;
using System.Security.Claims;
using BancoMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoMVC.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ClaimsPrincipal claimuser = HttpContext.User;
        string nombreUsuario = "";

        if (claimuser.Identity?.IsAuthenticated ?? false) {
            nombreUsuario = claimuser.Claims!.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault()!;
        }

        ViewData["nombreUsuario"] = nombreUsuario;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
