using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgileX.Infrastructure.Persistence.Configuration;

public class TicketConfigurations : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");

        builder.HasKey(ticket => ticket.TicketId);
        builder.Property(ticket => ticket.TicketId).ValueGeneratedNever().HasColumnName("id");

        builder.Property(ticket => ticket.ProjectId).IsRequired().HasColumnName("project_id");
        builder.Property(ticket => ticket.AssignedUserId).HasColumnName("assigned_user_id");
        builder.Property(ticket => ticket.SprintId).HasColumnName("sprint_id");
        builder.Property(ticket => ticket.ParentTicketId).HasColumnName("parent_ticket_id");
        builder.Property(ticket => ticket.Name).IsRequired().HasColumnName("name");
        builder.Property(ticket => ticket.Description).IsRequired().HasColumnName("description");

        builder
            .Property(ticket => ticket.Status)
            .IsRequired()
            .HasColumnName("status")
            .HasConversion(
                value => ConvertCompletionStatusToString(value),
                value => ConvertStringToCompletionStatus(value)
            );

        builder.Property(ticket => ticket.Deadline).IsRequired().HasColumnName("deadline");
        builder.Property(ticket => ticket.Priority).IsRequired().HasColumnName("priority");
        builder.Property(ticket => ticket.Reminder).IsRequired().HasColumnName("reminder");

        builder
            .Property(ticket => ticket.SubTicketsCount)
            .IsRequired()
            .HasColumnName("sub_tickets_count");

        builder
            .Property(ticket => ticket.CompletedSubTicketsCount)
            .IsRequired()
            .HasColumnName("completed_sub_tickets_count");

        builder
            .Property(ticket => ticket.CommentsCount)
            .IsRequired()
            .HasColumnName("comments_count");

        builder
            .Property(ticket => ticket.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");

        builder.Property(ticket => ticket.CreatedAt).IsRequired().HasColumnName("created_at");
        builder.Property(ticket => ticket.UpdatedAt).IsRequired().HasColumnName("updated_at");
        builder.Property(ticket => ticket.DeletedAt).HasColumnName("deleted_at");
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
