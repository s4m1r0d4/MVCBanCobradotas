using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BanCobradotas.Models;

[Table("Usuario")]
[Index("IDCuentaBancaria", IsUnique = true)]
[Index("IDCuentaIngreso", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public long IDUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string CURP { get; set; } = null!;

    public long IDEstado { get; set; }

    public long? IDCuentaIngreso { get; set; }

    public long? IDCuentaBancaria { get; set; }

    public long? IDNomina { get; set; }

    [ForeignKey("IDCuentaBancaria")]
    [InverseProperty("Usuario")]
    public virtual CuentaBancaria? IDCuentaBancariaNavigation { get; set; }

    [ForeignKey("IDCuentaIngreso")]
    [InverseProperty("UsuarioNavigation")]
    public virtual CuentaIngreso? IDCuentaIngresoNavigation { get; set; }

    [ForeignKey("IDEstado")]
    [InverseProperty("Usuarios")]
    public virtual Estado IDEstadoNavigation { get; set; } = null!;

    [ForeignKey("IDNomina")]
    [InverseProperty("Usuarios")]
    public virtual Nomina? IDNominaNavigation { get; set; }
}
