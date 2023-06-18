using BanCobradotas.Models;
using static System.Console;

using BanCobradotasContext db = new();

var usuarios = db.Usuarios.OrderBy(u => u.FechaNacimiento);

string line = new('-', 76);
WriteLine(line);
WriteLine($"|{"ID Usuario",-12}|{"Nombres",-30}|{"Apellidos",-30}|");
WriteLine(line);
foreach (var user in usuarios) {
    WriteLine($"|{user.IDUsuario,-12}|{user.Nombre,-30}|{user.Apellido,-30}|");
}
WriteLine(line);

