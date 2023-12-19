using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Queries.ListSprintTickets;

public record ListSprintTicketsQuery(Guid SprintId, Guid UserId) : IRequest<Result<List<Ticket>>>;
