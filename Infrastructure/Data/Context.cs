using Domain.AggregateModels.SessionAggregate;
using Infrastructure.Data.Configs;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <inheritdoc />
/// <summary>
///     Data base context
/// </summary>
internal sealed class Context:DbContext
{

    public Context(DbContextOptions<Context> options):base(options) { Database.EnsureCreated(); }
    public DbSet<Session> AuthorizedSessions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorizedSessionConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}