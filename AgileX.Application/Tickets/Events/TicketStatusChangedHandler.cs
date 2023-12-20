using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using MediatR;

namespace AgileX.Application.Tickets.Events;

public class TicketStatusChangedHandler : INotificationHandler<TicketCreated>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public TicketStatusChangedHandler(
        IProjectRepository projectRepository,
        ITicketRepository ticketRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _projectRepository = projectRepository;
        _ticketRepository = ticketRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(TicketCreated notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var existingTicket = _ticketRepository.GetById(notification.TicketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return;

        var existingProject = _projectRepository.GetById(existingTicket.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return;

        var tickets = _ticketRepository.ListByProjectId(existingProject.ProjectId);

        var enumerable = tickets as Ticket[] ?? tickets.ToArray();
        var completedTasksCount = enumerable.Count(x => x.Status == CompletionStatus.COMPLETED);
        var startedTasksCount = enumerable.Count(x => x.Status == CompletionStatus.IN_PROGRESS);

        _projectRepository.Save(
            existingProject with
            {
                CompletionStatus =
                    enumerable.Length == completedTasksCount
                        ? CompletionStatus.COMPLETED
                        : startedTasksCount > 0
                            ? CompletionStatus.IN_PROGRESS
                            : CompletionStatus.NOT_STARTED,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );
    }
}
