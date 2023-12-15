using AgileX.Domain.ObjectValues;

namespace AgileX.Domain.Events;

public record PermissionGranted(Guid ProjectId, Guid UserId, Permission Permission) : IEvent;

public record PermissionRevoked(Guid ProjectId, Guid UserId, Permission Permission) : IEvent;
