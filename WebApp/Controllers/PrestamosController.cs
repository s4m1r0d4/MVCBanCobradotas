using BanCobradotas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

[Authorize(Policy = "UsuarioOnly")]

public class PrestamosController : Controller
{
    private readonly IPrestamosService? service;
    

}