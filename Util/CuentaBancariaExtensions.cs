using BanCobradotas.Models;
using Microsoft.EntityFrameworkCore;

namespace BanCobradotas.Util;

public static class CuentaBancariaExtensions
{
    public static (int affected, long cuentaBancariaID) AddCuentaBancaria(
        this BanCobradotasContext db)
    {
        CuentaBancaria cb = new()
        {
            Saldo = 10_000D
        };

        db.CuentasBancaria.Add(cb);
        int affected = db.SaveChanges();

        if (affected == 0) return (0, 0);

        return (affected, cb.IDCuentaBancaria);
    }

    public static async Task<Prestamo?> GetPrestamoActivo(
        this BanCobradotasContext db,
        CuentaBancaria cuenta)
    {
        var prestamos = await db.Prestamos
                                .Where(p => p.IDCuentaBancaria == cuenta.IDCuentaBancaria)
                                .Where(p => p.FechaLiquidacion == null)
                                .Include(p => p.Pagos)
                                .ToListAsync();

        if (prestamos == null) return null;

        return prestamos.OrderByDescending(p => p.FechaAprobacion).FirstOrDefault();
    }
}