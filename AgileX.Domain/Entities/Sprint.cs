namespace AgileX.Domain.Entities;

public record Sprint(
    Guid SprintId,
    Guid ProjectId,
    string Name,
    string Description,
    long Duration,
    DateTime StartDate,
    DateTime EndDate,
    bool IsDeleted,
    DateTime? DeletedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
