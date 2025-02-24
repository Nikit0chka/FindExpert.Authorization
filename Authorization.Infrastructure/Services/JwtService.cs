using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authorization.Application.Abstractions;
using Authorization.Domain.AggregatesModel.UserAggregate;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.Infrastructure.Services;

/// <inheritdoc />
/// <summary>
///     Jwt service implementation
/// </summary>
internal sealed class JwtService:IJwtService
{
    private const string SecretKey = "your-256-bit-secrett-secretttttt";
    private const string ClaimIdType = "id";

    public string GenerateJwtAccessToken(User user)
    {
        var claims = new[]
                     {
                         new Claim(ClaimIdType, user.Id.ToString()),
                         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                     };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.Now.AddMinutes(30);

        var token = new JwtSecurityToken(
                                         claims: claims,
                                         expires: expiration,
                                         signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateJwtRefreshToken(User user)
    {
        var claims = new[]
                     {
                         new Claim(ClaimIdType, user.Id.ToString()),
                         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                     };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.Now.AddDays(5);

        var token = new JwtSecurityToken(
                                         claims: claims,
                                         expires: expiration,
                                         signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public bool IsTokenValid(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
                                   {
                                       ValidateIssuerSigningKey = true,
                                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                                       ValidateLifetime = true,
                                       ClockSkew = TimeSpan.Zero,
                                       ValidateAudience = false,
                                       ValidateIssuer = false
                                   };

        tokenHandler.ValidateToken(jwtToken, validationParameters, out _);
        return true;
    }

    public int GetUserIdFromToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var readToken = tokenHandler.ReadJwtToken(jwtToken);

        var userIdClaim = readToken.Claims.FirstOrDefault(static claim => claim.Type == ClaimIdType);

        if (userIdClaim is null)
            throw new SecurityTokenException("Invalid token");

        return Convert.ToInt32(userIdClaim.Value);
    }
}