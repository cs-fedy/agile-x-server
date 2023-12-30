using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Errors;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Dependencies.Commands.DeleteDependency;

public class DeleteDependencyCommandHandler
    : IRequestHandler<DeleteDependencyCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IDependencyRepository _dependencyRepository;

    public DeleteDependencyCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        ITicketRepository ticketRepository,
        IDependencyRepository dependencyRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _ticketRepository = ticketRepository;
        _dependencyRepository = dependencyRepository;
    }

    public async Task<Result<SuccessMessage>> Handle(
        DeleteDependencyCommand request,
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
                Permission.DeleteDependency
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction;
        }

        if (existingTicket.Status is CompletionStatus.COMPLETED or CompletionStatus.IN_PROGRESS)
            return DependencyErrors.TicketInProgressOrCompleted;

        var existingDependencyTicket = _ticketRepository.GetById(request.DependencyTicketId);
        if (existingDependencyTicket is null || existingDependencyTicket.IsDeleted)
            return TicketErrors.TicketNotFound with { Description = "Dependency ticket not found" };

        if (existingDependencyTicket.ProjectId != existingTicket.ProjectId)
            return Error.Validation(
                code: "Ticket.NotSameProject",
                description: "Ticket and its dependency does not belong to the same project"
            );

        var existingDependency = _dependencyRepository.Get(
            request.TicketId,
            request.DependencyTicketId
        );

        if (existingDependency is null || existingDependency.IsDeleted)
            return DependencyErrors.DependencyNotFound;

        _dependencyRepository.Delete(request.TicketId, request.DependencyTicketId);
        return new SuccessMessage("Dependency deleted successfully");
    }
}
