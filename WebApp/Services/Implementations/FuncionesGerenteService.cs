using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BanCobradotas.Models;
using Microsoft.EntityFrameworkCore;
using WebApp.Services.Interfaces;

namespace WebApp.Services.Implementations;

public class FuncionesGerenteService : IFuncionesGerenteService
{
    private readonly BanCobradotasContext db;

    public FuncionesGerenteService(BanCobradotasContext injectedContext)
    {
        db = injectedContext;
    }

    public async Task<string?> AcceptSolicitudUsuario(Usuario usr, Gerente gerente)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Usuario>? GetSolicitudesUsuario()
    {
        var res = db.Usuarios.Where(u => u.IDEstado == 1).AsAsyncEnumerable();
        return res;
    }
}