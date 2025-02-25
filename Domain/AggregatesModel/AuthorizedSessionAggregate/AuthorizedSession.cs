using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Domain.AggregatesModel.UserAggregate;

namespace Domain.AggregatesModel.AuthorizedSessionAggregate;

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
        Guard.Against.NullOrEmpty(refreshToken, nameof(refreshToken), "RefreshToken is required.");
        Guard.Against.LengthOutOfRange(refreshToken, 1, 64, nameof(refreshToken), "RefreshToken must be between 1 and 64 characters.");

        RefreshToken = refreshToken;
        UserId = Guard.Against.NegativeOrZero(userId, nameof(userId), "User id cannot be less than 0.");

        CreatedAt = DateTime.UtcNow;
    }

    public string RefreshToken { get; private set; }
    public int UserId { get; init; }
    public virtual User User { get; init; }
    public DateTime CreatedAt { get; init; }
}