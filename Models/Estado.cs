using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanCobradotas.Models;

[Table("Estado")]
public partial class Estado
{
    [Key]
    public long IDEstado { get; set; }

    public string Descripcion { get; set; } = null!;

    [InverseProperty("IDEstadoNavigation")]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    [InverseProperty("IDEstadoNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
