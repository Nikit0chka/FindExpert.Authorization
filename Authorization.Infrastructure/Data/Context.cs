using Authorization.Domain.AggregatesModel.AuthorizedSessionAggregate;
using Authorization.Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Infrastructure.Data;

/// <inheritdoc />
/// <summary>
///     Data base context
/// </summary>
internal sealed class Context:DbContext
{
    public Context(DbContextOptions<Context> options):base(options) { Database.EnsureCreated(); }

    public DbSet<User> Users { get; init; }
    public DbSet<AuthorizedSession> AuthorizedSessions { get; init; }
}