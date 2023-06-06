using BanCobradotas.Models;
using BanCobradotas.Util;
using WebApp.Services.Implementations;

namespace UnitTests;

public class DatabaseUnitTests
{
    [Fact]
    public void DatabaseConnection()
    {
        using BanCobradotasContext db = new();

        Assert.True(db.Database.CanConnect(), "Couldn't connect to database");
    }

    [Fact]
    public void EstadosCount()
    {
        // Given
        using BanCobradotasContext db = new();

        // When
        int expected = 6;
        int actual = db.Estados.Count();

        // Then
        Assert.True(actual == expected, "Table 'Estados' should have 6 values");
    }

    [Fact]
    public void AddCuentaIngreso()
    {
        // Given
        using BanCobradotasContext db = new();
        string usuario = "test";
        string contrasena = "test";

        // When
        (int affected, long IDCuentaIngreso) = db.AddCuentaIngreso(usuario, contrasena);

        // Then
        Assert.True(affected == 1, "Error adding new CuentaIngreso");

        // Remove test account
        if (affected == 1) {
            db.CuentasIngreso.Remove(db.CuentasIngreso.First(c => c.IDCuentaIngreso == IDCuentaIngreso));
            affected = db.SaveChanges();
            if (affected != 1) {
                throw new Exception($"Error removing row from CuentaIngreso with id {IDCuentaIngreso}");
            }
        }
    }

    [Fact]
    public void AddDuplicateUsernameInCuentaIngreso()
    {
        // Given
        using BanCobradotasContext db = new();
        string usuario = "dummy";
        string contrasena = "dummy";

        // When
        (int affected, long IDCuentaIngreso) = db.AddCuentaIngreso(usuario, contrasena);

        // Then
        Assert.True(affected == 0, "There should be a default dummy CuentaIngreso");
    }

    [Fact]
    public void LogInWithCorrectCredentials()
    {
        // Given
        using BanCobradotasContext db = new();
        string usuario = "dummy";
        string contrasena = "dummy";

        // When
        var cuenta = db.LogIn(usuario, contrasena);

        // Then
        Assert.True(cuenta != null, "Error logging into CuentaIngreso with correct credentials");
    }

    [Fact]
    public async void LogInWithIncorrectPassword()
    {
        // Given
        using BanCobradotasContext db = new();
        string usuario = "dummy";
        string contrasena = "incorrectpassword";

        // When
        var cuenta = await db.LogIn(usuario, contrasena);

        // Then
        Assert.True(cuenta == null, "Error logging into CuentaIngreso with incorrect password");
    }

    [Fact]
    public async void LogInWithIncorrectUsername()
    {
        // Given
        using BanCobradotasContext db = new();
        string usuario = "nonexistingusername";
        string contrasena = "whatever";

        // When
        var cuenta = await db.LogIn(usuario, contrasena);

        // Then
        Assert.True(cuenta == null, "Error logging into CuentaIngreso with incorrect username");
    }

    [Fact]
    public async void ExhaustLogInAttempts()
    {
        // Given
        using BanCobradotasContext db = new();
        string usuario = "dummy";
        string contrasena = "dummy";

        // When
        var cuenta = db.CuentasIngreso.First(c => c.Usuario == usuario) ?? throw new Exception(
                "Couldn't find dummy account while trying ExhaustLogInAttempts");
        // Reset failed attempts
        int affected;
        if (cuenta.NumInicioFallido != 0 || cuenta.FechaInicioFallido != null) {
            cuenta.NumInicioFallido = 0;
            cuenta.FechaInicioFallido = null;
            affected = db.SaveChanges();
            if (affected != 1) {
                throw new Exception("Error resetting failed attempts on dummy CuentaIngreso");
            }
        }

        // Three consecutive failed attempts
        for (int i = 0; i < 3; i++) {
            cuenta = await db.LogIn(usuario, "incorrectpassword");
            if (cuenta is not null) {
                throw new Exception("LogIn attempt should have been unsuccessfull");
            }
        }

        // Try loggin with correct password, now that it has exhausted it's attempts
        cuenta = await db.LogIn(usuario, contrasena);

        // Then
        Assert.True(cuenta == null, "Account should have been blocked on 3 failed attempts");

        // Reset attempts again ...
        cuenta = db.CuentasIngreso.First(c => c.Usuario == usuario);
        // Reset failed attempts
        cuenta.NumInicioFallido = 0;
        cuenta.FechaInicioFallido = null;
        affected = db.SaveChanges();
        if (affected != 1) {
            throw new Exception("Error resetting failed attempts on dummy CuentaIngreso");
        }
    }

    [Fact]
    public async void LogInAfterTimespanOfExhaustedAttempts()
    {
        // Given
        using BanCobradotasContext db = new();
        string usuario = "dummy";
        string contrasena = "dummy";

        // When
        var cuenta = db.CuentasIngreso.First(c => c.Usuario == usuario) ?? throw new Exception(
                "Couldn't find dummy account while trying ExhaustLogInAttempts");

        // Set failed attempts to MAX, 6 minutes ago
        int affected;
        var fecha = DateTime.Now - new TimeSpan(0, 6, 0);
        if (cuenta.NumInicioFallido != 3 || cuenta.FechaInicioFallido != fecha) {
            cuenta.NumInicioFallido = 3;
            cuenta.FechaInicioFallido = fecha;
            affected = db.SaveChanges();
            if (affected != 1) {
                throw new Exception("Error setting failed attempts on dummy CuentaIngreso");
            }
        }


        cuenta = await db.LogIn(usuario, contrasena);
        Assert.True(cuenta is not null,
            "Dummy account should have logged-in successfully after limit has passed");

        // Reset attempts again ...
        cuenta.NumInicioFallido = 0;
        cuenta.FechaInicioFallido = null;
        affected = db.SaveChanges();
        if (affected != 1) {
            throw new Exception("Error resetting failed attempts on dummy CuentaIngreso");
        }
    }

    [Fact]
    public void RegisterUsuarioTest()
    {
        // Given
        using BanCobradotasContext db = new();
        string nombre = "Giordanna Maria Goretti";
        string apellido = "Garcia Bojorquez";
        DateTime fechaNacimiento = new(2004, 4, 23);
        string curp = "GABG040423MJCRJRA2";

        // When
        var cuentaIngreso = db.RegisterUsuario(nombre, apellido, fechaNacimiento, curp);

        // Then
        Assert.True(cuentaIngreso.affected == 1, "Error registering User");
    }

    [Fact]
    public void AddEmpleadoTest()
    {
        // Given
        using BanCobradotasContext db = new();
        string nombre = "Giordanna Maria Goretti";
        string apellido = "García Bojorquez";
        DateTime fechaNacimiento = new(2004, 4, 23);

        // When
        var empleado = db.AddEmpleado(nombre, apellido, fechaNacimiento);

        // Then
        Assert.True(empleado.affected == 1, "Error registering Empleado");
    }

    [Fact]
    public void AddGerenteTest()
    {
        // Given
        using BanCobradotasContext db = new();
        string nombre = "Giordanna Maria Goretti";
        string apellido = "García Bojorquez";
        DateTime fechaNacimiento = new(2004, 4, 23);

        // When
        var gerente = db.AddGerente(nombre, apellido, fechaNacimiento);

        // Then
        Assert.True(gerente.affected == 1, "Error registering Gerente");
    }

    [Fact]
    public async void TestNavigationProperties()
    {
        // Given
        using BanCobradotasContext db = new();
        CuentaIngresoService service = new(db);
        string username = "GIGB4371";
        string pswd = "GIGB4377472";

        // When
        var cuenta = await service.GetCuentaIngreso(username, pswd);

        // Then
        Assert.True(cuenta is not null, "Couldn't find default account");
        Assert.True(cuenta.Gerente is not null, "This account should belong to a Gerente");
    }
}