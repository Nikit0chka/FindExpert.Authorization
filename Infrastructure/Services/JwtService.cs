using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Contracts;
using Ardalis.SharedKernel;
using Domain.AggregateModels;
using Domain.AggregateModels.SessionAggregate;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

/// <inheritdoc />
/// <summary>
///     Jwt service implementation
/// </summary>
internal sealed class JwtService(IReadRepository<Session> sessionRepository):IJwtService
{
    private const string SecretKey = "your-256-bit-secrett-secretttttt";

    private const string Issuer = "http://localhost:5001";

    //TODO: Поменять время истечения токена
    private const int AccessTokenExpirationMinutes = 30000;
    private const int RefreshTokenExpirationDays = 5;

    public string GenerateJwtAccessToken(int userId, IEnumerable<Role> roles)
    {
        var claims = new List<Claim>
                     {
                         new(ClaimTypes.NameIdentifier, userId.ToString()),
                         new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
                     };

        claims.AddRange(roles.Select(static role => new Claim(ClaimTypes.Role, role.Name)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.Now.AddMinutes(AccessTokenExpirationMinutes);

        var token = new JwtSecurityToken(
                                         Issuer,
                                         claims: claims,
                                         expires: expiration,
                                         signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateJwtRefreshToken(int userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
                     {
                         new(ClaimTypes.NameIdentifier, userId.ToString()),
                         new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
                     };

        var expiration = DateTime.Now.AddDays(RefreshTokenExpirationDays);

        var token = new JwtSecurityToken(
                                         Issuer,
                                         expires: expiration,
                                         claims: claims,
                                         signingCredentials:
                                         signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool IsRefreshTokenValid(string refreshToken)
    {
        try
        {
            ValidateRefreshToken(refreshToken);
            return true;
        }
        catch (SecurityTokenException)
        {
            return false;
        }
    }

    public int GetUserIdFromRefreshToken(string refreshToken)
    {
        var principal = ValidateRefreshToken(refreshToken);
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            throw new SecurityTokenException("Invalid user ID in refresh token");

        return userId;
    }

    private static ClaimsPrincipal ValidateRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var validationParameters = new TokenValidationParameters
                                       {
                                           ValidateIssuerSigningKey = true,
                                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                                           ValidateIssuer = true,
                                           ValidIssuer = Issuer,
                                           ValidateAudience = false,
                                           ValidateLifetime = true
                                       };

            return tokenHandler.ValidateToken(refreshToken, validationParameters, out _);
        }
        catch (Exception ex) when (ex is SecurityTokenException or ArgumentException)
        {
            throw new SecurityTokenException("Invalid refresh token", ex);
        }
    }
}