namespace AgileX.Domain.Events;

public record SprintDeleted(Guid SprintId) : IEvent;

public record NewSprintTicket(Guid TicketId) : IEvent;
