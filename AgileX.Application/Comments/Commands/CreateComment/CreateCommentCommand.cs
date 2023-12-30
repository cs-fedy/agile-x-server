using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Comments.Commands.CreateComment;

public abstract record CreateCommentCommand(
    Guid TicketId,
    Guid UserId,
    Guid? ParentCommentId,
    string Text,
    string? AttachedCode
) : IRequest<Result<SuccessMessage>>;
