using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Queries.ListMemberTickets;

public class ListMemberTicketsQueryHandler
    : IRequestHandler<ListMemberTicketsQuery, Result<List<Ticket>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ITicketRepository _ticketRepository;

    public ListMemberTicketsQueryHandler(
        IProjectRepository projectRepository,
        IUserRepository userRepository,
        IMemberRepository memberRepository,
        ITicketRepository ticketRepository
    )
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _memberRepository = memberRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<List<Ticket>>> Handle(
        ListMemberTicketsQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingProject = _projectRepository.GetById(request.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingLoggedMember = _memberRepository.Get(request.ProjectId, request.LoggedUserId);
        if (existingLoggedMember is null || existingLoggedMember.IsDeleted)
            return MemberErrors.UnauthorizedMember with
            {
                Description = "Logged user is not a member"
            };

        var existingTargetUser = _userRepository.GetById(request.TargetUserId);
        if (existingTargetUser is null || existingTargetUser.IsDeleted)
            return UserErrors.UserNotFound with { Description = "Target user not found" };

        var existingTargetMember = _memberRepository.Get(request.ProjectId, request.TargetUserId);
        if (existingTargetMember is null || existingTargetMember.IsDeleted)
            return MemberErrors.UnauthorizedMember with
            {
                Description = "Target user is not a member"
            };

        return _ticketRepository
            .ListByAssignedUserId(request.TargetUserId)
            .Where(x => !x.IsDeleted)
            .ToList();
    }
}
