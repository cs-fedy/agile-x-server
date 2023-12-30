using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgileX.Infrastructure.Persistence.Configuration;

public class ProjectConfigurations : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");

        builder.HasKey(project => project.ProjectId);
        builder.Property(project => project.ProjectId).ValueGeneratedNever().HasColumnName("id");

        builder.Property(project => project.Name).IsRequired().HasColumnName("name");
        builder.Property(project => project.Description).IsRequired().HasColumnName("description");

        builder
            .Property(project => project.CompletionStatus)
            .IsRequired()
            .HasColumnName("completion_status")
            .HasConversion(
                value => ConvertCompletionStatusToString(value),
                value => ConvertStringToCompletionStatus(value)
            );

        builder.Property(x => x.Progress).IsRequired().HasColumnName("progress");
        builder.Property(x => x.Priority).IsRequired().HasColumnName("priority");
        builder.Property(x => x.Deadline).IsRequired().HasColumnName("deadline");
        builder.Property(user => user.IsDeleted).HasDefaultValue(false).HasColumnName("is_deleted");
        builder.Property(user => user.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(user => user.UpdatedAt).IsRequired().HasColumnName("updated_at");
        builder.Property(user => user.DeletedAt).HasColumnName("deleted_at");
    }

    private static string ConvertCompletionStatusToString(CompletionStatus completionStatus)
    {
        return completionStatus switch
        {
            CompletionStatus.COMPLETED => "completed",
            CompletionStatus.IN_PROGRESS => "in_progress",
            CompletionStatus.NOT_STARTED => "not_started",
            _ => "not_started"
        };
    }

    private static CompletionStatus ConvertStringToCompletionStatus(string rawCompletionStatus)
    {
        return rawCompletionStatus switch
        {
            "completed" => CompletionStatus.COMPLETED,
            "in_progress" => CompletionStatus.IN_PROGRESS,
            "not_started" => CompletionStatus.NOT_STARTED,
            _ => CompletionStatus.NOT_STARTED
        };
    }
}
