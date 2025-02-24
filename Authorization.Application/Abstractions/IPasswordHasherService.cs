namespace Authorization.Application.Abstractions;

/// <summary>
///     Logic for password hashing
/// </summary>
public interface IPasswordHasherService
{
    /// <summary>
    ///     Password hashing logic
    /// </summary>
    /// <param name="password"> Password to hash </param>
    /// <returns> Hashed password </returns>
    public string HashPassword(string password);

    /// <summary>
    ///     Compare hashed and not hashed passwords
    /// </summary>
    /// <param name="password"> Not hashed password </param>
    /// <param name="hashedPassword"> Hashed password </param>
    /// <returns> Is password equals </returns>
    public bool VerifyPassword(string password, string hashedPassword);
}