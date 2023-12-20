using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Dependencies.Commands.AddDependency;

public class AddDependencyCommandHandler
    : IRequestHandler<AddDependencyCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberPermissionRepository _memberPermissionRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IDependencyRepository _dependencyRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AddDependencyCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IMemberPermissionRepository memberPermissionRepository,
        ITicketRepository ticketRepository,
        IDependencyRepository dependencyRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _memberPermissionRepository = memberPermissionRepository;
        _ticketRepository = ticketRepository;
        _dependencyRepository = dependencyRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        AddDependencyCommand request,
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
                Permission.ADD_DEPENDENCY
            );

            if (existingPermission is null || existingPermission.IsDeleted)
                return PermissionErrors.UnauthorizedAction;
        }

        if (existingTicket.Status == CompletionStatus.COMPLETED)
            return DependencyErrors.TicketCompleted;

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

        if (existingDependency is not null)
            return DependencyErrors.DependencyAlreadyExist;

        _dependencyRepository.Save(
            new Dependency(
                TicketId: request.TicketId,
                DependencyTicketId: request.DependencyTicketId,
                CreatedAt: _dateTimeProvider.UtcNow,
                IsDeleted: false,
                DeletedAt: null
            )
        );

        return new SuccessMessage("Dependency added successfully");
    }
}
