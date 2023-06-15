using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BanCobradotas.Models;
using Microsoft.EntityFrameworkCore;
using WebApp.Services.Interfaces;

namespace WebApp.Services.Implementations;

// Implementación de la interfaz ITestService, debe implementar todos los métodos de ITestService

// IMPORTANTE, una vez definida la implementación, añadir en el Program.cs la línea:
// builder.Services.AddScoped<ITestService, TestService>();
// ITestService es la Interfaz del servicio
// TestService es la implementación de la interfaz
public class TestService : ITestService
{
    private readonly BanCobradotasContext db;

    // Constructor
    public TestService(BanCobradotasContext injectedContext)
    {
        // Agarra automáticamente una instancia de dbcontext para acceder a la base de datos
        db = injectedContext;
    }

    // Método asíncrono debe de tener la keyword "async"
    public async Task<List<Boleto>?> GetBoletos(Prestamo prestamo)
    {
        var boletos = await db.Boletos.ToListAsync();
        return boletos;
    }
}