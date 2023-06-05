using BanCobradotas.Models;

namespace BanCobradotas.Util;

public static class EmpleadoExtensions
{

    public static (int affected, long empleadoID) AddEmpleado(
        this BanCobradotasContext db,
        string nombre,
        string apellido,
        DateTime fechaNacimiento)
    {
        var cuentaIngreso = db.AddCuentaIngreso(nombre, apellido, fechaNacimiento);
        if (cuentaIngreso.affected == 0) return (0, 0);

        var nomina = db.AddNomina(DateTime.Now);
        if (nomina.affected == 0) return (0, 0);

        Empleado empleado = new()
        {
            Nombre = nombre,
            Apellido = apellido,
            FechaNacimiento = fechaNacimiento,
            IDNomina = nomina.nominaID,
            IDCuentaIngreso = cuentaIngreso.cuentaIngresoID,
        };

        db.Empleados.Add(empleado);
        int affected = db.SaveChanges();

        return (affected, empleado.IDEmpleado);
    }
}
