using Authorization.Domain.AggregatesModel.AuthorizedSessionAggregate;
using Authorization.Domain.AggregatesModel.UserAggregate;
using Authorization.Infrastructure.Data.Configs;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new AuthorizedSessionConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}