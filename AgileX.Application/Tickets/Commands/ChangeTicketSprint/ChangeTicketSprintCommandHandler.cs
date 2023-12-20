using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Commands.ChangeTicketSprint;

public class ChangeTicketSprintCommandHandler
    : IRequestHandler<ChangeTicketSprintCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEventProvider _eventProvider;

    public ChangeTicketSprintCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        ISprintRepository sprintRepository,
        IDateTimeProvider dateTimeProvider,
        ITicketRepository ticketRepository,
        IEventProvider eventProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _sprintRepository = sprintRepository;
        _dateTimeProvider = dateTimeProvider;
        _ticketRepository = ticketRepository;
        _eventProvider = eventProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        ChangeTicketSprintCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingTicket = _ticketRepository.GetById(request.TicketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return TicketErrors.TicketNotFound;

        if (existingTicket.SprintId == request.NewSprintId)
            return Error.Validation(description: "Sprint already linked with the sprint");

        var existingProject = _projectRepository.GetById(existingTicket.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMember = _memberRepository.Get(existingProject.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        if (existingMember.Membership == Membership.PROJECT_MEMBER)
        {
            var existingPermission = _memberPermissionRepository.Get(
                existingProject.ProjectId,
                request.UserId,
                Permission.CHANGE_TICKET_SPRINT
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction;
        }

        var existingSprint = _sprintRepository.GetById(request.NewSprintId);
        if (existingSprint is null || existingSprint.IsDeleted)
            return SprintErrors.SprintNotFound;

        _ticketRepository.Save(
            existingTicket with
            {
                SprintId = request.NewSprintId,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );

        await _eventProvider.Publish(new NewSprintTicket(TicketId: existingTicket.TicketId));

        return new SuccessMessage("Ticket sprint changed successfully");
    }
}
