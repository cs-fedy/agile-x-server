using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Tickets.Queries.ListSprintTickets;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Queries.ListSubTickets;

public class ListSubTicketsQueryHandler : IRequestHandler<ListSubTicketsQuery, Result<List<Ticket>>>
{
    private IProjectRepository _projectRepository;
    private ITicketRepository _ticketRepository;
    private IMemberRepository _memberRepository;

    public ListSubTicketsQueryHandler(
        IProjectRepository projectRepository,
        ITicketRepository ticketRepository,
        IMemberRepository memberRepository
    )
    {
        _projectRepository = projectRepository;
        _ticketRepository = ticketRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<List<Ticket>>> Handle(
        ListSubTicketsQuery request,
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

        return _ticketRepository
            .ListByParentTicketId(request.TicketId)
            .Where(x => !x.IsDeleted)
            .ToList();
    }
}
