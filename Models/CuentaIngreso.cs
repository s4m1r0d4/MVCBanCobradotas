using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanCobradotas.Models;

[Table("CuentaIngreso")]
[Index("Usuario", IsUnique = true)]
public partial class CuentaIngreso
{
    [Key]
    public long IDCuentaIngreso { get; set; }

    public string Usuario { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public DateTime? FechaInicioFallido { get; set; }

    public long NumInicioFallido { get; set; }

    [InverseProperty("IDCuentaIngresoNavigation")]
    public virtual Empleado? Empleado { get; set; }

    [InverseProperty("IDCuentaIngresoNavigation")]
    public virtual Gerente? Gerente { get; set; }

    [InverseProperty("IDCuentaIngresoNavigation")]
    public virtual Usuario? UsuarioNavigation { get; set; }
}
