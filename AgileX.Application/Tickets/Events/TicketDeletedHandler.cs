using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using MediatR;

namespace AgileX.Application.Tickets.Events;

public class TicketDeletedHandler : INotificationHandler<TicketDeleted>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public TicketDeletedHandler(
        IProjectRepository projectRepository,
        ITicketRepository ticketRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _projectRepository = projectRepository;
        _ticketRepository = ticketRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(TicketDeleted notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var existingTicket = _ticketRepository.GetById(notification.TicketId);
        if (existingTicket is null)
            return;

        await HandleProjectProgressChanges(existingTicket.ProjectId);

        if (existingTicket.ParentTicketId is not null)
            await HandleParentTicketCompletionStatus(existingTicket.ParentTicketId.Value);
    }

    private async Task HandleProjectProgressChanges(Guid projectId)
    {
        await Task.CompletedTask;
        var existingProject = _projectRepository.GetById(projectId);
        if (existingProject is null || existingProject.IsDeleted)
            return;

        var tickets = _ticketRepository.ListByProjectId(existingProject.ProjectId);

        var enumerable = tickets as Ticket[] ?? tickets.ToArray();
        var completedTicketsCount = enumerable.Count(x => x.Status == CompletionStatus.COMPLETED);
        var startedTicketsCount = enumerable.Count(x => x.Status == CompletionStatus.IN_PROGRESS);

        _projectRepository.Save(
            existingProject with
            {
                CompletionStatus =
                    enumerable.Length == completedTicketsCount
                        ? CompletionStatus.COMPLETED
                        : startedTicketsCount > 0
                            ? CompletionStatus.IN_PROGRESS
                            : CompletionStatus.NOT_STARTED,
                Progress = (decimal)((completedTicketsCount * 1.0) / enumerable.Length * 100),
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );
    }

    private async Task HandleParentTicketCompletionStatus(Guid ticketId)
    {
        await Task.CompletedTask;
        var existingTicket = _ticketRepository.GetById(ticketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return;

        var subTickets = _ticketRepository.ListByParentTicketId(ticketId);

        var enumerable = subTickets as Ticket[] ?? subTickets.ToArray();
        var completedTicketsCount = enumerable.Count(x => x.Status == CompletionStatus.COMPLETED);

        _ticketRepository.Save(
            existingTicket with
            {
                CompletedSubTicketsCount = completedTicketsCount,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );
    }
}
