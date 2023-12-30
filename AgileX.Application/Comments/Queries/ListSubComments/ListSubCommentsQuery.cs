using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Comments.Queries.ListSubComments;

public abstract record ListSubCommentsQuery(Guid ParentCommentId, Guid UserId)
    : IRequest<Result<List<Comment>>>;
