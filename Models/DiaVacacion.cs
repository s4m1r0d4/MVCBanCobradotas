using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanCobradotas.Models;

[Table("DiaVacacion")]
public partial class DiaVacacion
{
    [Key]
    public long IDDiaVacacion { get; set; }

    public DateTime Fecha { get; set; }

    public long? IDGerente { get; set; }

    [ForeignKey("IDGerente")]
    [InverseProperty("DiaVacacions")]
    public virtual Gerente? IDGerenteNavigation { get; set; }
}
