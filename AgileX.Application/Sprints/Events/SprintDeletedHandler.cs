using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Events;
using MediatR;

namespace AgileX.Application.Sprints.Events;

public class SprintDeletedHandler : INotificationHandler<SprintDeleted>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SprintDeletedHandler(
        ISprintRepository sprintRepository,
        ITicketRepository ticketRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _sprintRepository = sprintRepository;
        _ticketRepository = ticketRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(SprintDeleted notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var existingSprint = _sprintRepository.GetById(notification.SprintId);
        if (existingSprint is null)
            return;

        var tickets = _ticketRepository.ListBySprintId(notification.SprintId);
        foreach (var ticket in tickets)
            _ticketRepository.Save(
                ticket with
                {
                    SprintId = null,
                    UpdatedAt = _dateTimeProvider.UtcNow
                }
            );
    }
}
