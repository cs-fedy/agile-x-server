using AgileX.Domain.ObjectValues;

namespace AgileX.Domain.Entities;

public record Member(
    Guid UserId,
    Guid ProjectId,
    Membership Membership,
    bool IsDeleted,
    DateTime? DeletedAt,
    DateTime CreatedAt
);
