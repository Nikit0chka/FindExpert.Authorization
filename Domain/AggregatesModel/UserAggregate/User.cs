using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Domain.AggregatesModel.AuthorizedSessionAggregate;

namespace Domain.AggregatesModel.UserAggregate;

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
        Guard.Against.NullOrEmpty(login, nameof(login), "Login is required.");
        Guard.Against.LengthOutOfRange(login, 1, 64, nameof(login), "Login must be between 1 and 64 characters.");
        Login = login;

        Guard.Against.NullOrEmpty(passwordHash, nameof(passwordHash), "Password is required.");
        Guard.Against.LengthOutOfRange(passwordHash, 1, 64, nameof(passwordHash), "Password must be between 1 and 64 characters.");
        PasswordHash = passwordHash;

        CreatedAt = DateTime.UtcNow;
        AuthorizedSessions = new List<AuthorizedSession>();
    }

    public string Login { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public virtual ICollection<AuthorizedSession> AuthorizedSessions { get; private set; }
}