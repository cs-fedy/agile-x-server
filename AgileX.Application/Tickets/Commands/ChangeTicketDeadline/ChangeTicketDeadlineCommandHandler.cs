using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Commands.ChangeTicketDeadline;

public class ChangeTicketDeadlineCommandHandler
    : IRequestHandler<ChangeTicketDeadlineCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEventProvider _eventProvider;

    public ChangeTicketDeadlineCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        ITicketRepository ticketRepository,
        IDateTimeProvider dateTimeProvider,
        IEventProvider eventProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _ticketRepository = ticketRepository;
        _dateTimeProvider = dateTimeProvider;
        _eventProvider = eventProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        ChangeTicketDeadlineCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingTicket = _ticketRepository.GetById(request.TicketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return TicketErrors.TicketNotFound;

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
                Permission.ChangeTicketDeadline
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction;
        }

        _ticketRepository.Save(
            existingTicket with
            {
                Deadline = request.NewDeadline,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );

        await _eventProvider.Publish(new TicketDeadlineChanged(TicketId: request.TicketId));

        return new SuccessMessage("Ticket's deadline changed successfully");
    }
}
