using AgileX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgileX.Infrastructure.Persistence.Configuration;

public class SprintConfigurations : IEntityTypeConfiguration<Sprint>
{
    public void Configure(EntityTypeBuilder<Sprint> builder)
    {
        builder.ToTable("sprints");

        builder.HasKey(sprint => sprint.SprintId);
        builder.Property(sprint => sprint.SprintId).ValueGeneratedNever().HasColumnName("id");

        builder.Property(sprint => sprint.ProjectId).IsRequired().HasColumnName("project_id");
        builder.Property(sprint => sprint.Name).IsRequired().HasColumnName("name");
        builder.Property(sprint => sprint.Description).IsRequired().HasColumnName("description");
        builder.Property(sprint => sprint.Duration).IsRequired().HasColumnName("duration");
        builder.Property(sprint => sprint.StartDate).IsRequired().HasColumnName("start_date");
        builder.Property(sprint => sprint.EndDate).IsRequired().HasColumnName("end_date");

        builder
            .Property(sprint => sprint.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");

        builder.Property(sprint => sprint.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(sprint => sprint.UpdatedAt).IsRequired().HasColumnName("updated_at");
        builder.Property(sprint => sprint.DeletedAt).HasColumnName("deleted_at");
    }
}
