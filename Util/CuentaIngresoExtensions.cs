using System.Reflection.Metadata;
using System.Security.Principal;
using System.Text;
using BanCobradotas.Models;
using Microsoft.EntityFrameworkCore;

namespace BanCobradotas.Util;

public static class CuentaIngresoExtensions
{
    public static (int affected, long cuentaIngresoID) AddCuentaIngreso(
        this BanCobradotasContext db,
        string usuario,
        string contrasena)
    {
        // It should be FirstOrDefault because it can be null
        if (db.CuentasIngreso.Any() && db.CuentasIngreso.FirstOrDefault(c => c.Usuario == usuario) != null)
            return (0, 0);

        string encryptedPassword = Cryptography.HashSHA256(contrasena);
        CuentaIngreso cuentaIngreso = new()
        {
            Usuario = usuario,
            Contrasena = encryptedPassword
        };

        db.CuentasIngreso.Add(cuentaIngreso);
        int affected = db.SaveChanges();

        return (affected, cuentaIngreso.IDCuentaIngreso);
    }

    public static (int affected, long cuentaIngresoID) AddCuentaIngreso(
        this BanCobradotasContext db,
        string nombre,
        string apellido,
        DateTime fechaNacimiento)
    {
        string[] apellidos = apellido.Split(' ');

        StringBuilder part = new();
        part.Append(nombre[0..2].ToUpper());
        part.Append(apellidos[0][0..1].ToUpper());
        part.Append(apellidos[1][0..1].ToUpper());

        int hash1 = DateTime.Now.GetHashCode();
        if (hash1 < 0) hash1 *= -1; // Always positive hash

        int hash2 = fechaNacimiento.GetHashCode();
        if (hash2 < 0) hash2 *= -1; // Always positive hash

        StringBuilder usuario = new();
        usuario.Append(part);
        usuario.Append(hash2 % 1000);
        usuario.Append(hash1 % 10);

        StringBuilder contrasena = new();
        contrasena.Append(part);
        contrasena.Append(hash2 % 1000);
        contrasena.Append(hash1 % 10000 + 1); // pa dar lata, nombre bien seguro

#warning Credentials are being logged
        string dir = Environment.CurrentDirectory;
        string path = string.Empty;
        if (dir.EndsWith("net7.0")) { // In case program is being tested (bin/debug/net7.0)
            path = Path.Combine("..", "..", "..", "..", "cuentas_debug.txt");
        } else {
            path = Path.Combine("..", "cuentas_debug.txt");
        }
        using (StreamWriter txtStream = new(path, true)) {
            txtStream.WriteLine($"[{usuario}, {contrasena}]");
        }

        return db.AddCuentaIngreso(usuario.ToString(), contrasena.ToString());
    }

    public static async Task<CuentaIngreso?> LogIn(
        this BanCobradotasContext db,
        string usuario,
        string contrasena)
    {
        // It should be FirstOrDefault because it can be null
        var cuenta = await db.CuentasIngreso.FirstOrDefaultAsync(c => c.Usuario == usuario);
        if (cuenta is null) return null;

        var diff = DateTime.Now - cuenta.FechaInicioFallido;
        var fiveMinutes = new TimeSpan(0, 5, 0);

        if (cuenta.FechaInicioFallido is not null) {
            if (diff < fiveMinutes && cuenta.NumInicioFallido == 3) {
                // Maximum of 3 failed attempts in 5 minutes
                return null;
            }
        }

        string encryptedPassword = Cryptography.HashSHA256(contrasena);
        if (cuenta.Contrasena == encryptedPassword) {
            // Successful attempt
            return cuenta;
        }

        // Failed attempt
        if (diff < fiveMinutes) {
            ++cuenta.NumInicioFallido;
        } else {
            // Reset failed attempts, timespan of 5 minutes since last attempt has passed
            cuenta.FechaInicioFallido = DateTime.Now;
            cuenta.NumInicioFallido = 1;
        }
        int affected = await db.SaveChangesAsync();
        if (affected != 1) {
            throw new Exception(
                $"Error, couldn't update user with ID {cuenta.IDCuentaIngreso} on failed LogIn attempt");
        }

        return null;
    }
}