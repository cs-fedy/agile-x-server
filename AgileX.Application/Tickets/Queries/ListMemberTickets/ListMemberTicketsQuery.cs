using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Queries.ListMemberTickets;

public abstract record ListMemberTicketsQuery(Guid ProjectId, Guid LoggedUserId, Guid TargetUserId)
    : IRequest<Result<List<Ticket>>>;
