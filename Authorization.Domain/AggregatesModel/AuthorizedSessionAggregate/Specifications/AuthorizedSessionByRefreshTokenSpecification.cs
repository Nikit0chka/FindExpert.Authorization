using Ardalis.Specification;

namespace Authorization.Domain.AggregatesModel.AuthorizedSessionAggregate.Specifications;

public sealed class AuthorizedSessionByRefreshTokenSpecification:Specification<AuthorizedSession>, ISingleResultSpecification<AuthorizedSession>
{
    public AuthorizedSessionByRefreshTokenSpecification(string refreshToken) { Query.Where(authorizedSession => authorizedSession.RefreshToken == refreshToken); }
}