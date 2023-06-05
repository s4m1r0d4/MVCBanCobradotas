using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanCobradotas.Models;

[Table("Boleto")]
public partial class Boleto
{
    [Key]
    public long IDBoleto { get; set; }

    public DateTime Fecha { get; set; }

    public long? IDPrestamo { get; set; }

    [ForeignKey("IDPrestamo")]
    [InverseProperty("Boletos")]
    public virtual Prestamo? IDPrestamoNavigation { get; set; }
}
