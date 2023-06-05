using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BanCobradotas.Models;

[Table("CuentaBancaria")]
public partial class CuentaBancaria
{
    [Key]
    public long IDCuentaBancaria { get; set; }

    public double Saldo { get; set; }

    [InverseProperty("IDCuentaBancariaNavigation")]
    public virtual Gerente? Gerente { get; set; }

    [InverseProperty("IDCuentaBancariaNavigation")]
    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    [InverseProperty("IDCuentaBancariaNavigation")]
    public virtual Usuario? Usuario { get; set; }
}
