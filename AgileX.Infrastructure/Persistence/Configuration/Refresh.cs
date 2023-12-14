using AgileX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgileX.Infrastructure.Persistence.Configuration;

public class RefreshConfigurations : IEntityTypeConfiguration<Refresh>
{
    public void Configure(EntityTypeBuilder<Refresh> builder)
    {
        builder.ToTable("refreshes");

        builder.HasKey(refresh => refresh.Token);
        builder.Property(refresh => refresh.Token).ValueGeneratedNever().HasColumnName("token");
        builder.Property(refresh => refresh.OwnerId).IsRequired().HasColumnName("owner_id");
        builder.Property(refresh => refresh.ExpiresIn).IsRequired().HasColumnName("expires_in");
        // TODO: specify owner_id as a foreign key
    }
}
