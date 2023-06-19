using BanCobradotas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
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

var prestamo = await db.Prestamos.Where(p => p.IDPrestamo == 4 && p.FechaAprobacion != null).FirstOrDefaultAsync();
if (prestamo is null) {
    WriteLine("Error finding the prestamo");
    return;
}

List<Pago> pagos = new();
for (int i = 0; i < 6; i++) {
    Pago p = new()
    {
        Fecha = DateTime.Now,
        Cantidad = (double)prestamo.PagoMensual,
        Numero = i + 1,
        IDPrestamo = prestamo.IDPrestamo
    };

    pagos.Add(p);
}

await db.Pagos.AddRangeAsync(pagos);
int affected = await db.SaveChangesAsync();
if (affected != 6)
    WriteLine("Error añadiendo pagos");