using Domain.AggregateModels.SessionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configs;

/// <inheritdoc />
/// <summary>
///     Authorized session entity ef core configuration <see cref="Session" /> />
/// </summary>
internal class AuthorizedSessionConfiguration:IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.Property(static session => session.RefreshToken)
            .HasMaxLength(SessionConstants.TokenMaxLength)
            .IsRequired();
    }
}