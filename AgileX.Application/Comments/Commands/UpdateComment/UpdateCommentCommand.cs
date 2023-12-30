using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Comments.Commands.UpdateComment;

public abstract record UpdateCommentCommand(
    Guid CommentId,
    Guid UserId,
    string? Text,
    string? AttachedCode
) : IRequest<Result<SuccessMessage>>;
