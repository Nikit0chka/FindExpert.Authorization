using Ardalis.Specification;

namespace Domain.AggregatesModel.UserAggregate.Specifications;

/// <inheritdoc cref="Ardalis.Specification.Specification{T}" />
/// <summary>
///     User specification by login
/// </summary>
public sealed class UserByLoginSpecification:Specification<User>, ISingleResultSpecification<User>
{
    public UserByLoginSpecification(string login) { Query.Where(user => user.Login == login); }
}