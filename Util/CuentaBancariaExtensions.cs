using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BanCobradotas.Models;

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
}