using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BanCobradotas.Models;

namespace WebApp.Services.Interfaces;

public interface ITestService
{
    Task<List<Boleto>?> GetBoletos(Prestamo prestamo);
}