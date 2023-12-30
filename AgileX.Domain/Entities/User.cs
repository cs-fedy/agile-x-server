using AgileX.Domain.ObjectValues;

namespace AgileX.Domain.Entities;

public record User(
    Guid UserId,
    string Email,
    string Password,
    string FullName,
    string Username,
    Role Role,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsConfirmed,
    bool IsDeleted,
    DateTime? DeletedAt
);
