using Ardalis.SharedKernel;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <inheritdoc cref="Ardalis.Specification.EntityFrameworkCore.RepositoryBase{T}" />
/// <summary>
///     Base ef repository implementation
/// </summary>
/// <param name="dbContext"> Data base context </param>
/// <typeparam name="T"> Type of entity </typeparam>
internal sealed class EfRepository<T>(DbContext dbContext):
    RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot;