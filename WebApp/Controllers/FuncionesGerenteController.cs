using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BanCobradotas.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

[Authorize(Policy = "GerenteOnly")]
public class FuncionesGerenteController : Controller
{
    private readonly IFuncionesGerenteService service;

    public FuncionesGerenteController(IFuncionesGerenteService injectedService)
    {
        service = injectedService;
    }

    public async Task<IActionResult> SolicitudesUsuario()
    {
        var solicitudes = await service.GetSolicitudesUsuario();

        var model = new SolicitudesUsuarioModel()
        {
            SolicitudesUsuario = solicitudes
        };

        return View(model);
    }

}