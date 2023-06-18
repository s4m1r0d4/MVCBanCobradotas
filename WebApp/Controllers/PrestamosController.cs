using BanCobradotas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Interfaces;
using System.Security.Claims;

namespace WebApp.Controllers;

[Authorize(Policy = "UsuarioOnly")]

public class PrestamosController : Controller
{
    private readonly BanCobradotasContext db;

    public PrestamosController(BanCobradotasContext injectedContext)
    {
        db = injectedContext;
    }
    //verificar que el saldo en la cuenta sea 10,000MXN o mas.


}