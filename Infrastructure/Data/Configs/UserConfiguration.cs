using Domain.AggregatesModel.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configs;

public class UserConfiguration:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(static user => user.PasswordHash)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(static user => user.Login)
            .HasMaxLength(64)
            .IsRequired();

        builder.HasMany(static user => user.AuthorizedSessions).WithOne(static session => session.User);
    }
}