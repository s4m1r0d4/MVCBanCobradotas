using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BanCobradotas.Models;
using Microsoft.EntityFrameworkCore;
using WebApp.Services.Interfaces;

namespace WebApp.Services.Implementations;

public class PrestamoService : IPrestamosService
{
    private readonly BanCobradotasContext db;

    public PrestamoService(BanCobradotasContext injectedContext)
    {
        db = injectedContext;
    }
}