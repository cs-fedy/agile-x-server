using AgileX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgileX.Infrastructure.Persistence.Configuration;

public class LabelConfigurations : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        builder.ToTable("labels");

        builder.HasKey(label => label.LabelId);
        builder.Property(label => label.LabelId).ValueGeneratedNever().HasColumnName("id");

        builder.Property(label => label.ProjectId).IsRequired().HasColumnName("project_id");
        builder.Property(label => label.TicketId).IsRequired().HasColumnName("ticket_id");
        builder.Property(label => label.Content).IsRequired().HasColumnName("content");

        builder
            .Property(label => label.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");

        builder.Property(label => label.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(label => label.DeletedAt).HasColumnName("deleted_at");
    }
}
