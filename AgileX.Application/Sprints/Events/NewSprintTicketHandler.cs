using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Events;
using MediatR;

namespace AgileX.Application.Sprints.Events;

public class NewSprintTicketHandler : INotificationHandler<NewSprintTicket>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public NewSprintTicketHandler(
        ITicketRepository ticketRepository,
        ISprintRepository sprintRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _ticketRepository = ticketRepository;
        _sprintRepository = sprintRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(NewSprintTicket notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var existingTicket = _ticketRepository.GetById(notification.TicketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return;

        if (existingTicket.SprintId is null)
            return;

        var existingSprint = _sprintRepository.GetById(existingTicket.SprintId.Value);
        if (existingSprint is null || existingSprint.IsDeleted)
            return;

        var dates = new List<DateTime>() { existingTicket.Deadline, existingSprint.EndDate };
        var updatedEndDate = dates.Max();

        _sprintRepository.Save(
            existingSprint with
            {
                EndDate = updatedEndDate,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );
    }
}
