using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Authorization.Domain.AggregatesModel.UserAggregate;

namespace Authorization.Domain.AggregatesModel.AuthorizedSessionAggregate;

/// <inheritdoc cref="Ardalis.SharedKernel.EntityBase" />
/// <summary>
///     Authorized session entity
/// </summary>
public class AuthorizedSession:EntityBase, IAggregateRoot
{
    /// <summary>
    ///     Ef constructor
    /// </summary>
    protected AuthorizedSession() { }

    public AuthorizedSession(string refreshToken, int userId)
    {
        RefreshToken = Guard.Against.NullOrEmpty(refreshToken, nameof(refreshToken), "Refresh token is required.");
        UserId = Guard.Against.NegativeOrZero(userId, nameof(userId), "User id cannot be less than 0");
    }

    public string RefreshToken { get; private set; }
    public int UserId { get; init; }
    public virtual User User { get; init; } = null!;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}