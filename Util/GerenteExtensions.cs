using BanCobradotas.Models;

namespace BanCobradotas.Util;

public static class GerenteExtensions
{

    public static (int affected, long cuentaIngresoID) AddGerente(
        this BanCobradotasContext db,
        string nombre,
        string apellido,
        DateTime fechaNacimiento)
    {

        var nomina = db.AddNomina(DateTime.Now);
        if (nomina.affected == 0) return (0, 0);

        var cuentaIngreso = db.AddCuentaIngreso(nombre, apellido, fechaNacimiento);
        if (cuentaIngreso.affected == 0) return (0, 0);

        var cuentaBancaria = db.AddCuentaBancaria();
        if (cuentaBancaria.affected == 0) return (0, 0);

        Gerente gerente = new()
        {
            Nombre = nombre,
            Apellido = apellido,
            FechaNacimiento = fechaNacimiento,
            IDNomina = nomina.nominaID,
            IDCuentaIngreso = cuentaIngreso.cuentaIngresoID,
            IDCuentaBancaria = cuentaBancaria.cuentaBancariaID
        };

        db.Gerentes.Add(gerente);
        int affected = db.SaveChanges();

        return (affected, gerente.IDGerente);
    }
}
