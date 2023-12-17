namespace AgileX.Domain.Events;

public record MemberJoined(Guid ProjectId, Guid UserId, Guid AddedBy) : IEvent;

public record MemberRemoved(Guid ProjectId, Guid UserId, Guid AddedBy) : IEvent;
