using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanCobradotas.Models;

[Table("Empleado")]
[Index("IDCuentaIngreso", IsUnique = true)]
[Index("IDNomina", IsUnique = true)]
public partial class Empleado
{
    [Key]
    public long IDEmpleado { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public long IDCuentaIngreso { get; set; }

    public long IDNomina { get; set; }

    [ForeignKey("IDCuentaIngreso")]
    [InverseProperty("Empleado")]
    public virtual CuentaIngreso IDCuentaIngresoNavigation { get; set; } = null!;

    [ForeignKey("IDNomina")]
    [InverseProperty("Empleado")]
    public virtual Nomina IDNominaNavigation { get; set; } = null!;
}
