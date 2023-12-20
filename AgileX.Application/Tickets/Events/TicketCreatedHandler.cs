using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using MediatR;

namespace AgileX.Application.Tickets.Events;

public class TicketCreatedHandler : INotificationHandler<TicketCreated>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEventProvider _eventProvider;
    private readonly IEventBus _eventBus;

    public TicketCreatedHandler(
        IProjectRepository projectRepository,
        ITicketRepository ticketRepository,
        IUserRepository userRepository,
        IDateTimeProvider dateTimeProvider,
        IEventProvider eventProvider,
        IEventBus eventBus
    )
    {
        _projectRepository = projectRepository;
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
        _eventProvider = eventProvider;
        _eventBus = eventBus;
    }

    public async Task Handle(TicketCreated notification, CancellationToken cancellationToken)
    {
        var existingTicket = _ticketRepository.GetById(notification.TicketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return;

        if (existingTicket.AssignedUserId is null)
            return;

        var existingAssigneeUser = _userRepository.GetById(notification.CreatedBy);
        if (existingAssigneeUser is null || existingAssigneeUser.IsDeleted)
            return;

        var existingProject = _projectRepository.GetById(existingTicket.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return;

        var existingUser = _userRepository.GetById(existingTicket.AssignedUserId.Value);
        if (existingUser is null || existingUser.IsDeleted)
            return;

        _projectRepository.Save(
            existingProject with
            {
                CompletionStatus = CompletionStatus.IN_PROGRESS,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );

        if (existingTicket.ParentTicketId is not null)
            await HandleNewSubTicket(existingTicket.ParentTicketId.Value);

        if (existingTicket.SprintId is not null)
            await _eventProvider.Publish(new NewSprintTicket(TicketId: existingTicket.TicketId));

        await _eventBus.Publish(
            new NewEmail(
                new Email(
                    To: new List<string>() { existingUser.Email },
                    Subject: "Got Assigned a ticket",
                    PlainTextContent: $"Dear {existingUser.FullName}. \n"
                        + "You got assigned the newly created ticket: "
                        + $"`{existingTicket.Name}` as you are a member of the "
                        + $"`{existingProject.Name}` project by {existingAssigneeUser.FullName}."
                )
            ),
            cancellationToken
        );
    }

    private async Task HandleNewSubTicket(Guid parentTicket)
    {
        await Task.CompletedTask;
        var existingTicket = _ticketRepository.GetById(parentTicket);
        if (existingTicket is null || existingTicket.IsDeleted)
            return;

        _ticketRepository.Save(
            existingTicket with
            {
                SubTicketsCount = existingTicket.SubTicketsCount + 1,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );
    }
}
