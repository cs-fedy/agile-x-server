namespace AgileX.Domain.Events;

public record ProjectDeleted(Guid ProjectId, Guid DeletedBy) : IEvent;
