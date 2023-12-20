namespace AgileX.Domain.Events;

public record TicketCreated(Guid TicketId, Guid CreatedBy) : IEvent;

public record TicketStatusChanged(Guid TicketId) : IEvent;

public record TicketDeadlineChanged(Guid TicketId) : IEvent;

public record TicketDeleted(Guid TicketId) : IEvent;
