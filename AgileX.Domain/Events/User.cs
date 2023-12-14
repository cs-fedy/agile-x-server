namespace AgileX.Domain.Events;

public record UserCreated(Guid UserId) : IEvent;
