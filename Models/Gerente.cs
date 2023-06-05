using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanCobradotas.Models;

[Table("Gerente")]
[Index("IDCuentaBancaria", IsUnique = true)]
[Index("IDCuentaIngreso", IsUnique = true)]
[Index("IDNomina", IsUnique = true)]
public partial class Gerente
{
    [Key]
    public long IDGerente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public long IDNomina { get; set; }

    public long IDCuentaIngreso { get; set; }

    public long IDCuentaBancaria { get; set; }

    [InverseProperty("IDGerenteNavigation")]
    public virtual ICollection<DiaVacacion> DiaVacacions { get; set; } = new List<DiaVacacion>();

    [ForeignKey("IDCuentaBancaria")]
    [InverseProperty("Gerente")]
    public virtual CuentaBancaria IDCuentaBancariaNavigation { get; set; } = null!;

    [ForeignKey("IDCuentaIngreso")]
    [InverseProperty("Gerente")]
    public virtual CuentaIngreso IDCuentaIngresoNavigation { get; set; } = null!;

    [ForeignKey("IDNomina")]
    [InverseProperty("Gerente")]
    public virtual Nomina IDNominaNavigation { get; set; } = null!;
}
