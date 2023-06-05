using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanCobradotas.Models;

[Table("Nomina")]
public partial class Nomina
{
    [Key]
    public long IDNomina { get; set; }

    public DateTime FechaIngreso { get; set; }

    [InverseProperty("IDNominaNavigation")]
    public virtual Empleado? Empleado { get; set; }

    [InverseProperty("IDNominaNavigation")]
    public virtual Gerente? Gerente { get; set; }

    [InverseProperty("IDNominaNavigation")]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    [InverseProperty("IDNominaNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
