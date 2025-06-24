using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Domain.Extensions;
using JetBrains.Annotations;

namespace Domain.AggregateModels.SessionAggregate;

/// <inheritdoc cref="Ardalis.SharedKernel.EntityBase" />
/// <summary>
///     Authorized session entity
/// </summary>
public sealed class Session:EntityBase, IAggregateRoot
{

    /// <summary>
    ///     Ef constructor
    /// </summary>
    [UsedImplicitly]
    private Session() { }

    public Session(string refreshToken, int userId)
    {
        Guard.Against.NullOrEmpty(refreshToken, nameof(refreshToken), SessionValidationMessages.RefreshTokenIsRequired);

        GuardAgainstExtensions.StringLengthOutOfRange(refreshToken,
                                                      SessionConstants.TokenMinLength,
                                                      SessionConstants.TokenMaxLength,
                                                      nameof(refreshToken),
                                                      SessionValidationMessages.RefreshTokenIsOutOfRange
                                                     );

        RefreshToken = refreshToken;
        UserId = Guard.Against.NegativeOrZero(userId, nameof(userId), SessionValidationMessages.UserIdCannotBeLessThanZero);

        CreatedAt = DateTime.UtcNow;
    }

    public string RefreshToken { get; private set; }
    public int UserId { get; init; }
    public DateTime CreatedAt { get; init; }
}