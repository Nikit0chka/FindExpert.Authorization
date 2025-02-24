using System.Security.Cryptography;
using System.Text;
using Authorization.Application.Abstractions;
using Konscious.Security.Cryptography;

namespace Authorization.Infrastructure.Services;

/// <inheritdoc />
/// <summary>
///     Password hasher service implementation
/// </summary>
public sealed class PasswordHasherService:IPasswordHasherService
{
    private const int Iterations = 4;
    private const int MemorySize = 1024 * 1024;
    private const int DegreeOfParallelism = 8;
    private const int SaltSize = 16;

    public string HashPassword(string password)
    {
        var salt = new byte[SaltSize];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                     {
                         Salt = salt,
                         DegreeOfParallelism = DegreeOfParallelism,
                         Iterations = Iterations,
                         MemorySize = MemorySize
                     };

        var hash = argon2.GetBytes(32);

        var hashBytes = new byte[SaltSize + 32];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, hashBytes, SaltSize, 32);

        return Convert.ToBase64String(hashBytes);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var hashBytes = Convert.FromBase64String(hashedPassword);

        var salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, SaltSize);

        var storedHash = new byte[32];
        Buffer.BlockCopy(hashBytes, SaltSize, storedHash, 0, 32);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                     {
                         Salt = salt,
                         DegreeOfParallelism = DegreeOfParallelism,
                         Iterations = Iterations,
                         MemorySize = MemorySize
                     };

        var computedHash = argon2.GetBytes(32);

        return CompareByteArrays(storedHash, computedHash);
    }

    private static bool CompareByteArrays(byte[] array1, byte[] array2)
    {
        if (array1.Length != array2.Length)
            return false;

        return !array1.Where((t, i) => t != array2[i]).Any();
    }
}