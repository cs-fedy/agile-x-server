namespace AgileX.Domain.Events;

public record TicketCreated(Guid TicketId, Guid CreatedBy) : IEvent;
