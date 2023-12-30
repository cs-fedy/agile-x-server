using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Commands.CreateTicket;

public class CreateTicketCommandHandler
    : IRequestHandler<CreateTicketCommand, Result<SuccessMessage>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEventProvider _eventProvider;

    public CreateTicketCommandHandler(
        ITicketRepository ticketRepository,
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        ISprintRepository sprintRepository,
        IUserRepository userRepository,
        IDateTimeProvider dateTimeProvider,
        IEventProvider eventProvider
    )
    {
        _ticketRepository = ticketRepository;
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _sprintRepository = sprintRepository;
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
        _eventProvider = eventProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        CreateTicketCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingProject = _projectRepository.GetById(request.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMember = _memberRepository.Get(request.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember with
            {
                Description = "Logged user is not a member in this project"
            };

        if (existingMember.Membership == Membership.PROJECT_MEMBER)
        {
            var existingPermission = _memberPermissionRepository.Get(
                request.ProjectId,
                request.UserId,
                Permission.CreateTicket
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction with
                {
                    Description = "Logged user is not authorized to perform this action"
                };
        }

        if (request.AssignedUserId != null)
        {
            var existingTargetUser = _userRepository.GetById(request.AssignedUserId.Value);
            if (existingTargetUser is null || existingTargetUser.IsDeleted)
                return UserErrors.UserNotFound with { Description = "Target user not found" };

            var existingTargetMember = _memberRepository.Get(
                request.ProjectId,
                request.AssignedUserId.Value
            );
            if (existingTargetMember is null || existingTargetMember.IsDeleted)
                return MemberErrors.UnauthorizedMember with
                {
                    Description = "Target user is not a project member"
                };
        }

        if (request.SprintId != null)
        {
            var existingSprint = _sprintRepository.GetById(request.SprintId.Value);
            if (existingSprint is null || existingSprint.IsDeleted)
                return SprintErrors.SprintNotFound;
        }

        if (request.ParentTicketId != null)
        {
            var existingTicket = _ticketRepository.GetById(request.ParentTicketId.Value);
            if (existingTicket is null || existingTicket.IsDeleted)
                return TicketErrors.TicketNotFound with { Description = "Parent ticket not found" };
        }

        var creationDate = _dateTimeProvider.UtcNow;
        var ticketId = Guid.NewGuid();

        _ticketRepository.Save(
            new Ticket(
                TicketId: ticketId,
                ProjectId: request.ProjectId,
                AssignedUserId: request.AssignedUserId,
                SprintId: request.SprintId,
                ParentTicketId: request.ParentTicketId,
                Name: request.Name,
                Description: request.Description,
                Status: CompletionStatus.NOT_STARTED,
                Deadline: request.Deadline,
                Priority: request.Priority,
                Reminder: request.Reminder,
                SubTicketsCount: 0,
                CompletedSubTicketsCount: 0,
                CommentsCount: 0,
                IsDeleted: false,
                DeletedAt: null,
                CreatedAt: creationDate,
                UpdatedAt: creationDate
            )
        );

        await _eventProvider.Publish(
            new TicketCreated(TicketId: ticketId, CreatedBy: request.UserId)
        );

        return new SuccessMessage("Task created successfully");
    }
}
