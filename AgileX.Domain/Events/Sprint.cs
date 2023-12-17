namespace AgileX.Domain.Events;

public record SprintDeleted(Guid SprintId) : IEvent;
