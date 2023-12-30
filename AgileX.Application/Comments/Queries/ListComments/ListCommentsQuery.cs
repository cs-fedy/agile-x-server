using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Comments.Queries.ListComments;

public abstract record ListCommentsQuery(Guid TicketId, Guid UserId)
    : IRequest<Result<List<Comment>>>;
