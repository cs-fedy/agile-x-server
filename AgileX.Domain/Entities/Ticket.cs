using AgileX.Domain.ObjectValues;

namespace AgileX.Domain.Entities;

public record Ticket(
    Guid TicketId,
    Guid ProjectId,
    Guid? AssignedUserId,
    Guid? SprintId,
    Guid? ParentTicketId,
    string Name,
    string Description,
    CompletionStatus Status,
    DateTime Deadline,
    int Priority,
    int Reminder,
    int SubTicketsCount,
    int CompletedSubTicketsCount,
    int CommentsCount,
    bool IsDeleted,
    DateTime? DeletedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
