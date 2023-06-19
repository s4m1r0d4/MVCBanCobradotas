using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanCobradotas.Models;

[Table("Prestamo")]
public partial class Prestamo
{
    [Key]
    [Display(Name = "ID Presamo")]
    public long IDPrestamo { get; set; }

    [Display(Name = "Fecha de Solicitud")]
    public DateTime FechaSolicitud { get; set; }

    [Display(Name = "Fecha de Aprobacion")]
    public DateTime? FechaAprobacion { get; set; }

    [Display(Name = "Fecha de Liquidación")]
    public DateTime? FechaLiquidacion { get; set; }

    [Display(Name = "Número de meses")]
    public long NumMeses { get; set; }

    [Display(Name = "Pago Mensual")]
    public double? PagoMensual { get; set; }

    [Display(Name = "Interés")]
    public double? Interes { get; set; }

    [Display(Name = "Cantidad")]
    public double Cantidad { get; set; }

    [Display(Name = "Estado")]
    public long IDEstado { get; set; }

    [Display(Name = "ID Cuenta Bancaria")]
    public long IDCuentaBancaria { get; set; }

    [Display(Name = "ID Nómina")]
    public long? IDNomina { get; set; }

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
    public virtual Nomina? IDNominaNavigation { get; set; }

    [InverseProperty("IDPrestamoNavigation")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
