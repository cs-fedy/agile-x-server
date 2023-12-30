using AgileX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgileX.Infrastructure.Persistence.Configuration;

public class DependencyConfigurations : IEntityTypeConfiguration<Dependency>
{
    public void Configure(EntityTypeBuilder<Dependency> builder)
    {
        builder.ToTable("dependencies");

        builder.Property(dependency => dependency.TicketId).IsRequired().HasColumnName("ticket_id");

        builder
            .Property(dependency => dependency.DependencyTicketId)
            .IsRequired()
            .HasColumnName("dependency_ticket_id");

        builder.HasKey(dependency => new { dependency.TicketId, dependency.DependencyTicketId });

        builder
            .Property(dependency => dependency.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");

        builder
            .Property(dependency => dependency.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(dependency => dependency.DeletedAt).HasColumnName("deleted_at");
    }
}
