using System.Text;
using BanCobradotas.Models;

namespace BanCobradotas.Util;

public static class Validator
{
    public static bool ValidName(string name)
    {
        return name.All(c => char.IsLetter(c) || c == ' ');
    }

    public static bool ValidCURP(Usuario usr)
    {
        string[] apellidos = usr.Apellido.Trim().ToUpper().Split(' ');
        string nombre = usr.Nombre.Trim().ToUpper();
        string[] clavesEntidades = { "AS", "BC", "BS", "CC", "CL", "CM", "CS", "CH", "DF", "DG", "GT", "GR", "HG", "JC", "MC", "MN", "MS", "NT", "NL", "OC", "PL", "QT", "QR", "SP", "SL", "SR", "TC", "TS", "TL", "VZ", "YN", "ZS", "NE" };
        string vocales = "AEIOU";

        // Longitud de 18 caracteres
        if (usr.CURP.Length != 18) return false;

        // Primera letra del primer apellido.
        if (!(usr.CURP[0] == apellidos[0][0])) return false;

        // Primera vocal del primer apellido.
        if (vocales.Contains(apellidos[0][0])) {
            if (!(usr.CURP[1] == apellidos[0][1..].First(vocales.Contains))) return false;
        } else {
            if (!(usr.CURP[1] == apellidos[0].First(vocales.Contains))) return false;
        }

        // Primera letra del segundo apellido.
        if (!(usr.CURP[2] == apellidos[1][0])) return false;

        // Primera letra del nombre de pila
        if (!(usr.CURP[3] == nombre[0])) return false;

        // Fecha de nacimiento sin espacios en orden de año (dos dígitos), mes y día. Ejemplo: 960917 (17 de septiembre de 1996).
        StringBuilder date = new();
        int year = usr.FechaNacimiento.Year % 100;
        if (year < 10) date.Append('0');
        date.Append(year);
        if (usr.FechaNacimiento.Month < 10) date.Append('0');
        date.Append(usr.FechaNacimiento.Month);
        if (usr.FechaNacimiento.Day < 10) date.Append('0');
        date.Append(usr.FechaNacimiento.Day);

        if (!(usr.CURP.Substring(4, 6) == date.ToString())) return false;

        // Letra del sexo o género (H para Hombre, M para Mujer, o X para No binario3​).
        if (!(usr.CURP[10] == 'H' || usr.CURP[10] == 'M')) return false;

        // Dos letras correspondientes a la entidad federativa de nacimiento (en caso de haber nacido fuera del país, se marca como NE, «Nacido en el Extranjero»; véase el Catálogo de claves de entidades federativas).
        string entidad = usr.CURP.Substring(11, 2);
        if (!clavesEntidades.Contains(entidad)) return false;

        // Primera consonante interna (después de la primera letra) del primer apellido.
        if (!(usr.CURP[13] == apellidos[0][1..].First(c => !vocales.Contains(c)))) return false;

        // Primera consonante interna del segundo apellido.
        if (!(usr.CURP[14] == apellidos[1][1..].First(c => !vocales.Contains(c)))) return false;

        // Primera consonante interna del nombre de pila.
        if (!(usr.CURP[15] == nombre[1..].First(c => !vocales.Contains(c)))) return false;

        // Dígito del 0 al 9 para fechas de nacimiento hasta el año 1999 y de la A a la Z para fechas de nacimiento a partir del 2000, asignado por la SEGOB para evitar registros repetidos.
        DateTime limit = new(2000, 1, 1);
        if (usr.FechaNacimiento < limit) {
            if (!char.IsDigit(usr.CURP[16])) return false;
        } else {
            if (!char.IsLetter(usr.CURP[16])) return false;
        }

        // Dígito verificador, para comprobar la integridad.
        if (!char.IsDigit(usr.CURP[17])) return false;

        return true;
    }

}