using Microsoft.EntityFrameworkCore;

namespace BanCobradotas.Models;

public partial class BanCobradotasContext : DbContext
{

#pragma warning disable CS8618
    public BanCobradotasContext()
    {
    }

    public BanCobradotasContext(DbContextOptions<BanCobradotasContext> options)
        : base(options)
    {
    }
#pragma warning restore CS8618

    public virtual DbSet<Boleto> Boletos { get; set; }

    public virtual DbSet<CuentaBancaria> CuentasBancaria { get; set; }

    public virtual DbSet<CuentaIngreso> CuentasIngreso { get; set; }

    public virtual DbSet<DiaVacacion> DiasVacacion { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Gerente> Gerentes { get; set; }

    public virtual DbSet<Nomina> Nominas { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Prestamo> Prestamos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) {
            return;
        }

        string fileName = "WebApp/BanCobradotas.db";
        string rootDirectory = Directory.GetParent(Environment.CurrentDirectory)!
                                    .Parent!
                                    .Parent!
                                    .Parent!
                                    .FullName;

        string path = Path.Combine(rootDirectory, fileName);
        string connectionString = $"Filename={path}";

        optionsBuilder.UseSqlite(connectionString);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Boleto>(entity =>
        {
            entity.HasOne(d => d.IDPrestamoNavigation).WithMany(p => p.Boletos).OnDelete(DeleteBehavior.SetNull);

            entity.Property(d => d.Fecha).HasColumnName("Fecha").HasConversion<DateTime>();
        });

        modelBuilder.Entity<CuentaBancaria>(entity =>
        {
            entity.Property(e => e.Saldo).HasDefaultValueSql("10000");
        });

        modelBuilder.Entity<CuentaIngreso>(entity =>
        {
            entity.Property(e => e.NumInicioFallido).HasDefaultValueSql("0");

            entity.Property(e => e.FechaInicioFallido).HasColumnName("FechaInicioFallido").HasConversion<DateTime?>();
        });

        modelBuilder.Entity<DiaVacacion>(entity =>
        {
            entity.HasOne(d => d.IDGerenteNavigation).WithMany(p => p.DiaVacacions).OnDelete(DeleteBehavior.Cascade);

            entity.Property(d => d.Fecha).HasColumnName("Fecha").HasConversion<DateTime>();
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasOne(d => d.IDCuentaIngresoNavigation).WithOne(p => p.Empleado).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IDNominaNavigation).WithOne(p => p.Empleado).OnDelete(DeleteBehavior.Restrict);

            entity.Property(d => d.FechaNacimiento).HasColumnName("FechaNacimiento").HasConversion<DateTime>();
        });

        modelBuilder.Entity<Gerente>(entity =>
        {
            entity.HasOne(d => d.IDCuentaBancariaNavigation).WithOne(p => p.Gerente).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IDCuentaIngresoNavigation).WithOne(p => p.Gerente).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IDNominaNavigation).WithOne(p => p.Gerente).OnDelete(DeleteBehavior.Restrict);

            entity.Property(d => d.FechaNacimiento).HasColumnName("FechaNacimiento").HasConversion<DateTime>();
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasOne(d => d.IDPrestamoNavigation).WithMany(p => p.Pagos).OnDelete(DeleteBehavior.SetNull);

            entity.Property(d => d.Fecha).HasColumnName("Fecha").HasConversion<DateTime>();
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasOne(d => d.IDCuentaBancariaNavigation).WithMany(p => p.Prestamos).OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.IDEstadoNavigation).WithMany(p => p.Prestamos).OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.IDNominaNavigation).WithMany(p => p.Prestamos).OnDelete(DeleteBehavior.SetNull);

            entity.Property(d => d.FechaSolicitud).HasColumnName("FechaSolicitud").HasConversion<DateTime>();

            entity.Property(d => d.FechaAprobacion).HasColumnName("FechaAprobacion").HasConversion<DateTime?>();

            entity.Property(d => d.FechaLiquidacion).HasColumnName("FechaLiquidacion").HasConversion<DateTime?>();
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasOne(d => d.IDCuentaBancariaNavigation).WithOne(p => p.Usuario).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IDCuentaIngresoNavigation).WithOne(p => p.UsuarioNavigation).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IDEstadoNavigation).WithMany(p => p.Usuarios).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.IDNominaNavigation).WithMany(p => p.Usuarios).OnDelete(DeleteBehavior.SetNull);

            entity.Property(d => d.FechaNacimiento).HasColumnName("FechaNacimiento").HasConversion<DateTime>();
        });

        modelBuilder.Entity<Nomina>(entity =>
        {
            entity.Property(d => d.FechaIngreso).HasColumnName("FechaIngreso").HasConversion<DateTime>();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
