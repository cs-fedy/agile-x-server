using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Tickets.Queries.ListSubTickets;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Queries.GetTicket;

public class GetTicketQueryHandler : IRequestHandler<GetTicketQuery, Result<Ticket>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IMemberRepository _memberRepository;

    public GetTicketQueryHandler(
        IProjectRepository projectRepository,
        ITicketRepository ticketRepository,
        IMemberRepository memberRepository
    )
    {
        _projectRepository = projectRepository;
        _ticketRepository = ticketRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<Ticket>> Handle(
        GetTicketQuery request,
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

        return existingTicket;
    }
}
