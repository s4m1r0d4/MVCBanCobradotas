using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BanCobradotas.Models;

namespace BanCobradotas.Util;

public static class NominaExtensions
{

    public static (int affected, long nominaID) AddNomina(
        this BanCobradotasContext db,
        DateTime fechaIngreso)
    {
        Nomina nomina = new()
        {
            FechaIngreso = fechaIngreso
        };

        db.Nominas.Add(nomina);
        int affected = db.SaveChanges();

        return (affected, nomina.IDNomina);
    }
}