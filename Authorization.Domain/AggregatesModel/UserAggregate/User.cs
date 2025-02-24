using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace Authorization.Domain.AggregatesModel.UserAggregate;

/// <inheritdoc cref="Ardalis.SharedKernel.EntityBase" />
/// <summary>
///     User entity
/// </summary>
public class User:EntityBase, IAggregateRoot
{
    /// <summary>
    ///     Ef constructor
    /// </summary>
    protected User() { }

    public User(string login, string passwordHash)
    {
        Login = Guard.Against.NullOrEmpty(login, nameof(login), "Login is required.");
        PasswordHash = Guard.Against.NullOrEmpty(passwordHash, nameof(passwordHash), "Password is required.");
    }

    public string Login { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}