using BanCobradotas.Models;

namespace BanCobradotas.Util;

public static class UsuarioExtensions
{
    public static (int affected, long usuarioID) RegisterUsuario(
        this BanCobradotasContext db,
        string nombre,
        string apellido,
        DateTime fechaNacimiento,
        string curp)
    {
        if (!Validator.ValidName(nombre)) return (0, 0);
        if (!Validator.ValidName(apellido)) return (0, 0);

        DateTime limit = new(1962, 1, 1);
        if (fechaNacimiento < limit) return (0, 0);

        Usuario usuario = new()
        {
            Nombre = nombre,
            Apellido = apellido,
            FechaNacimiento = fechaNacimiento,
            CURP = curp,
            IDEstado = 1, // Hardcoded "En espera", deal with it
        };

        if (!Validator.ValidCURP(usuario)) return (0, 0);

        db.Usuarios.Add(usuario);
        int affected = db.SaveChanges();

        return (affected, usuario.IDUsuario);
    }

}