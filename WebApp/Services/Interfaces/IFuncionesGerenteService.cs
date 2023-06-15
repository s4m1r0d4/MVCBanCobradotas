using BanCobradotas.Models;

namespace WebApp.Services.Interfaces;

public interface IFuncionesGerenteService
{
    Task<List<Usuario>?> GetSolicitudesUsuario();

    Task<string?> AcceptSolicitudUsuario(Usuario usr, Gerente gerente);

    Task<Usuario?> FindUsuario(long id);
}