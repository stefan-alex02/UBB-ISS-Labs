using System.Security.Cryptography;
using System.Text;

namespace Business.Utils;

public class PasswordService {
    public static bool VerifyPasswordHash(string password, string hash) {
        byte[] hashBytes = Convert.FromBase64String(hash);
        byte[] salt = hashBytes[..16];
        byte[] passwordHash = hashBytes[16..];

        using var hmac = new HMACSHA512(salt);
        byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }
}