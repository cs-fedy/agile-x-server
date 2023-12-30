using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Commands.ChangeSprintDates;

public class ChangeSprintDatesCommandHandler
    : IRequestHandler<ChangeSprintDatesCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ChangeSprintDatesCommandHandler(
        IProjectRepository projectRepository,
        ISprintRepository sprintRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        ITicketRepository ticketRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _projectRepository = projectRepository;
        _sprintRepository = sprintRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _ticketRepository = ticketRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        ChangeSprintDatesCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingSprint = _sprintRepository.GetById(request.SprintId);
        if (existingSprint is null || existingSprint.IsDeleted)
            return SprintErrors.SprintNotFound;

        var existingProject = _projectRepository.GetById(existingSprint.ProjectId);
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
                Permission.ChangeSprintDates
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction;
        }

        var tickets = _ticketRepository.ListBySprintId(existingSprint.SprintId);

        var updatedStartDate =
            request.NewStartDate is null || request.NewStartDate > tickets.First().Deadline
                ? existingSprint.StartDate
                : request.NewStartDate;

        var updatedEndDate =
            request.NewEndDate is null || request.NewEndDate < tickets.Last().Deadline
                ? existingSprint.EndDate
                : request.NewEndDate;

        var updatedDuration = updatedEndDate.Value.Subtract(updatedStartDate.Value).Ticks;

        _sprintRepository.Save(
            existingSprint with
            {
                StartDate = updatedStartDate.Value,
                EndDate = updatedEndDate.Value,
                Duration = updatedDuration,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );

        var deadlines = new List<DateTime>() { existingProject.Deadline, updatedEndDate.Value };

        _projectRepository.Save(
            existingProject with
            {
                Deadline = deadlines.Max(),
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );

        return new SuccessMessage("Sprint Dates changed successfully");
    }
}
