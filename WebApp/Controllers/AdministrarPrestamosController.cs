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
    //tasa de interes calculo
}