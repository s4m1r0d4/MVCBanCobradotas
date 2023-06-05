using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanCobradotas.Models;

[Table("Prestamo")]
public partial class Prestamo
{
    [Key]
    public long IDPrestamo { get; set; }

    public DateTime FechaSolicitud { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public DateTime? FechaLiquidacion { get; set; }

    public long NumMeses { get; set; }

    public double PagoMensual { get; set; }

    public double Interes { get; set; }

    public double Cantidad { get; set; }

    public long IDEstado { get; set; }

    public long IDCuentaBancaria { get; set; }

    public long IDNomina { get; set; }

    [InverseProperty("IDPrestamoNavigation")]
    public virtual ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();

    [ForeignKey("IDCuentaBancaria")]
    [InverseProperty("Prestamos")]
    public virtual CuentaBancaria IDCuentaBancariaNavigation { get; set; } = null!;

    [ForeignKey("IDEstado")]
    [InverseProperty("Prestamos")]
    public virtual Estado IDEstadoNavigation { get; set; } = null!;

    [ForeignKey("IDNomina")]
    [InverseProperty("Prestamos")]
    public virtual Nomina IDNominaNavigation { get; set; } = null!;

    [InverseProperty("IDPrestamoNavigation")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
