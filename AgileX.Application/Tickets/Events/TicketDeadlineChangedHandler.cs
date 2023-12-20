using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using MediatR;

namespace AgileX.Application.Tickets.Events;

public class TicketDeadlineChangedHandler : INotificationHandler<TicketDeadlineChanged>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ISprintRepository _sprintRepository;

    public TicketDeadlineChangedHandler(
        ITicketRepository ticketRepository,
        IProjectRepository projectRepository,
        IDateTimeProvider dateTimeProvider,
        ISprintRepository sprintRepository
    )
    {
        _ticketRepository = ticketRepository;
        _projectRepository = projectRepository;
        _dateTimeProvider = dateTimeProvider;
        _sprintRepository = sprintRepository;
    }

    public async Task Handle(
        TicketDeadlineChanged notification,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingTicket = _ticketRepository.GetById(notification.TicketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return;

        await HandleProjectDeadlineChanges(existingTicket.ProjectId, existingTicket.Deadline);

        if (existingTicket.SprintId is not null)
            await HandleSprintDurationChanges(
                existingTicket.SprintId.Value,
                existingTicket.Deadline
            );

        if (existingTicket.ParentTicketId is not null)
            await HandleParentTicketDeadlineChanges(
                existingTicket.ParentTicketId.Value,
                existingTicket.Deadline
            );
    }

    private async Task HandleProjectDeadlineChanges(Guid projectId, DateTime ticketDeadline)
    {
        await Task.CompletedTask;
        var existingProject = _projectRepository.GetById(projectId);
        if (existingProject is null || existingProject.IsDeleted)
            return;

        var deadlines = new List<DateTime>() { ticketDeadline, existingProject.Deadline };

        _projectRepository.Save(
            existingProject with
            {
                Deadline = deadlines.Max(),
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );
    }

    private async Task HandleSprintDurationChanges(Guid sprintId, DateTime ticketDeadline)
    {
        await Task.CompletedTask;
        var existingSprint = _sprintRepository.GetById(sprintId);
        if (existingSprint is null || existingSprint.IsDeleted)
            return;

        var fromMilliseconds = TimeSpan.FromMilliseconds(existingSprint.Duration);
        var sprintDeadline = existingSprint.StartDate.Add(fromMilliseconds);

        var deadlines = new List<DateTime>() { ticketDeadline, sprintDeadline };
        var latestDeadline = deadlines.Max();

        var updatedDuration = latestDeadline.Subtract(existingSprint.StartDate).Ticks;

        _sprintRepository.Save(
            existingSprint with
            {
                Duration = updatedDuration,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );
    }

    private async Task HandleParentTicketDeadlineChanges(
        Guid parentTicketId,
        DateTime ticketDeadline
    )
    {
        await Task.CompletedTask;
        var existingTicket = _ticketRepository.GetById(parentTicketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return;

        var deadlines = new List<DateTime>() { existingTicket.Deadline, ticketDeadline };

        _ticketRepository.Save(
            existingTicket with
            {
                Deadline = deadlines.Max(),
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );
    }
}
