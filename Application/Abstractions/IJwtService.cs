using Domain.AggregatesModel.UserAggregate;

namespace Application.Abstractions;

/// <summary>
///     Logic for jwt tokens
/// </summary>
public interface IJwtService
{
    /// <summary>
    ///     Generate access token by user
    /// </summary>
    /// <param name="user"> User for token info </param>
    /// <returns> Access token </returns>
    string GenerateJwtAccessToken(User user);

    /// <summary>
    ///     Generate refresh token by user
    /// </summary>
    /// <param name="user"> User for token info </param>
    /// <returns> Refresh token </returns>
    string GenerateJwtRefreshToken(User user);

    /// <summary>
    ///     Check is token valid
    /// </summary>
    /// <param name="jwtToken"> Token for check </param>
    /// <returns> Is token valid </returns>
    bool IsTokenValid(string jwtToken);

    /// <summary>
    ///     Get user id from token
    /// </summary>
    /// <param name="jwtToken"> Source token </param>
    /// <returns> User id </returns>
    int GetUserIdFromToken(string jwtToken);
}