using Ardalis.Specification;

namespace Domain.AggregateModels.SessionAggregate.Specifications;

/// <inheritdoc cref="Ardalis.Specification.ISingleResultSpecification{T}" />
/// <summary>
///     Authorized session specification by refresh token
/// </summary>
public sealed class SessionByRefreshTokenSpecification:SingleResultSpecification<Session>
{
    public SessionByRefreshTokenSpecification(string refreshToken) { Query.Where(authorizedSession => authorizedSession.RefreshToken == refreshToken); }
}