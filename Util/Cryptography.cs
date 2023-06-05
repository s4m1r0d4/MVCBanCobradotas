using System.Security.Cryptography;
using System.Text;

namespace BanCobradotas.Util;

public static class Cryptography
{

    public static string HashSHA256(string input)
    {
        using SHA256 hash = SHA256.Create();

        return String.Concat(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(input)
            ).Select(item => item.ToString("x2"))
        );
    }
}