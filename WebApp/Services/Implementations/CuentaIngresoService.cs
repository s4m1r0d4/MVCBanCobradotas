using BanCobradotas.Models;
using BanCobradotas.Util;
using Microsoft.EntityFrameworkCore;
using WebApp.Services.Interfaces;

namespace WebApp.Services.Implementations;

public class CuentaIngresoService : ICuentaIngresoService
{
    private readonly BanCobradotasContext db;

    public CuentaIngresoService(BanCobradotasContext injectedContext)
    {
        db = injectedContext;
    }

    public async Task<(CuentaIngreso? cuenta, string? err)> GetCuentaIngreso(string usuario, string contrasena)
    {
        usuario = usuario.Trim();
        contrasena = contrasena.Trim();

        var res = await db.LogIn(usuario, contrasena);

        return res;
    }

    public async Task<(Usuario? usr, string? err)> RequestCuentaUsuario(Usuario usr)
    {
        usr.CURP = usr.CURP.Trim();
        usr.Nombre = usr.Nombre.Trim();
        usr.Apellido = usr.Apellido.Trim();

        var result = db.RegisterUsuario(
            usr.Nombre,
            usr.Apellido,
            usr.FechaNacimiento,
            usr.CURP);

        if (result.affected != 1) return (null, result.err);

        var usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.IDUsuario == result.usuarioID);

        return (usuario, null);
    }
}