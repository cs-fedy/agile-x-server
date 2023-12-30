using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Comments.Commands.DeleteComment;

public abstract record DeleteCommentCommand(Guid CommentId, Guid UserId)
    : IRequest<Result<SuccessMessage>>;
