using BanCobradotas.Models;
using BanCobradotas.Util;

namespace UnitTests;

public class DataValidationUnitTests
{
    [Fact]
    public void TestValidCURP1()
    {
        Usuario usr = new()
        {
            Nombre = "Francisco",
            Apellido = "Ortega Aguilar",
            FechaNacimiento = new(1977, 10, 12),
            CURP = "OEAF771012HMCRGR09",
        };

        bool actual = Validator.ValidCURP(usr);
        bool expected = true;

        Assert.True(actual == expected, "CURP should be valid");
    }

    [Fact]
    public void TestValidCURP2()
    {
        Usuario usr = new()
        {
            Nombre = "Ruy Felipe",
            Apellido = "Padilla Martínez",
            FechaNacimiento = new(2004, 7, 7),
            CURP = "PAMR040707HJCDRYA5",
        };

        bool actual = Validator.ValidCURP(usr);
        bool expected = true;

        Assert.True(actual == expected, "CURP should be valid");
    }

    [Fact]
    public void TestValidCURP3()
    {
        Usuario usr = new()
        {
            Nombre = "Luis Gerardo",
            Apellido = "Gonzalez Gervacio",
            FechaNacimiento = new(2003, 10, 15),
            CURP = "GOGL031015HJCNRSA5",
        };

        bool actual = Validator.ValidCURP(usr);
        bool expected = true;

        Assert.True(actual == expected, "CURP should be valid");
    }

    [Fact]
    public void TestInvalidCURP()
    {
        Usuario usr = new()
        {
            Nombre = "Ruy Felipe",
            Apellido = "Padilla Martínez",
            FechaNacimiento = new(2004, 7, 7),
            CURP = "PAMR040707HXXDRYA5",
        };

        bool actual = Validator.ValidCURP(usr);
        bool expected = false;

        Assert.True(actual == expected, "CURP should be invalid");
    }
}