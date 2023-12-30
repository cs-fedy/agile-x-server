using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Comments.Queries.GetComment;

public abstract record GetCommentQuery(Guid CommentId, Guid UserId) : IRequest<Result<Comment>>;
