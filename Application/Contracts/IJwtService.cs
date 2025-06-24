using Domain.AggregateModels;

namespace Application.Contracts;

/// <summary>
///     Logic for jwt tokens
/// </summary>
public interface IJwtService
{
    /// <summary>
    ///     Generate access token with payload by user
    /// </summary>
    /// <param name="userId"> Userid for token claims </param>
    /// <param name="roles"> User roles for token claims </param>
    /// <returns> Access token </returns>
    string GenerateJwtAccessToken(int userId, IEnumerable<Role> roles);

    /// <summary>
    ///     Generate refresh token by user
    /// </summary>
    /// <returns> Refresh token </returns>
    string GenerateJwtRefreshToken(int userId);

    /// <summary>
    ///     Check is token valid
    /// </summary>
    /// <param name="jwtToken"> Token for check </param>
    /// <returns> Is token valid </returns>
    bool IsRefreshTokenValid(string jwtToken);

    /// <summary>
    /// Get token claims from refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns>User id and session id</returns>
    int GetUserIdFromRefreshToken(string refreshToken);
}