using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Commands.ChangeTicketStatus;

public class ChangeTicketStatusCommandHandler
    : IRequestHandler<ChangeTicketStatusCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEventProvider _eventProvider;

    public ChangeTicketStatusCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ITicketRepository ticketRepository,
        IDateTimeProvider dateTimeProvider,
        IEventProvider eventProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _ticketRepository = ticketRepository;
        _dateTimeProvider = dateTimeProvider;
        _eventProvider = eventProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        ChangeTicketStatusCommand request,
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

        if (existingTicket.AssignedUserId != request.UserId)
            return TicketErrors.UnauthorizedMember;

        // TODO: - can't set task status to in_progress or completed when the dependencies are not completed
        _ticketRepository.Save(
            existingTicket with
            {
                Status = request.CompletionStatus,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );

        await _eventProvider.Publish(new TicketStatusChanged(TicketId: request.TicketId));

        return new SuccessMessage("Ticket completion status changed");
    }
}
