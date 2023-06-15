using BanCobradotas.Models;

namespace WebApp.Services.Interfaces
{
    public interface IFuncionesGerenteService
    {
        IAsyncEnumerable<Usuario>? GetSolicitudesUsuario();

        Task<string?> AcceptSolicitudUsuario(Usuario usr, Gerente gerente);
    }
}