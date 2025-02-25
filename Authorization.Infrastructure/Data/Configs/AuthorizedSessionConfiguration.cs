using Authorization.Domain.AggregatesModel.AuthorizedSessionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.Infrastructure.Data.Configs;

public class AuthorizedSessionConfiguration:IEntityTypeConfiguration<AuthorizedSession>
{
    public void Configure(EntityTypeBuilder<AuthorizedSession> builder)
    {
        builder.Property(static session => session.RefreshToken)
            .HasMaxLength(64)
            .IsRequired();

        builder.HasOne(static session => session.User).WithMany(static user => user.AuthorizedSessions);
    }
}