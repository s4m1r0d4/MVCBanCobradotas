using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanCobradotas.Models;

[Table("Pago")]
public partial class Pago
{
    [Key]
    public long IDPago { get; set; }

    public DateTime Fecha { get; set; }

    public double Cantidad { get; set; }

    public long Numero { get; set; }

    public long? IDPrestamo { get; set; }

    [ForeignKey("IDPrestamo")]
    [InverseProperty("Pagos")]
    public virtual Prestamo? IDPrestamoNavigation { get; set; }
}
