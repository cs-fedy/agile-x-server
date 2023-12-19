using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Queries.ListSprintTickets;

public class ListSprintTicketsQueryHandler
    : IRequestHandler<ListSprintTicketsQuery, Result<List<Ticket>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ISprintRepository _sprintRepository;
    private readonly ITicketRepository _ticketRepository;

    public ListSprintTicketsQueryHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ISprintRepository sprintRepository,
        ITicketRepository ticketRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _sprintRepository = sprintRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<List<Ticket>>> Handle(
        ListSprintTicketsQuery request,
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

        var existingMember = _memberRepository.Get(existingSprint.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        return _ticketRepository.ListBySprintId(request.SprintId).Where(x => !x.IsDeleted).ToList();
    }
}
