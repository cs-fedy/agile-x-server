using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Events;
using MediatR;

namespace AgileX.Application.Comments.Events;

public class CommentCreatedHandler : INotificationHandler<CommentCreated>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CommentCreatedHandler(
        ICommentRepository commentRepository,
        ITicketRepository ticketRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _commentRepository = commentRepository;
        _ticketRepository = ticketRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Handle(CommentCreated notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var existingComment = _commentRepository.GetById(notification.CommentId);
        if (existingComment is null || existingComment.IsDelete)
            return;

        var existingTicket = _ticketRepository.GetById(existingComment.TicketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return;

        if (existingComment.ParentCommentId is not null)
        {
            var existingParentComment = _commentRepository.GetById(
                existingComment.ParentCommentId.Value
            );
            if (existingParentComment is not null && !existingParentComment.IsDelete)
            {
                _commentRepository.Save(
                    existingParentComment with
                    {
                        SubCommentsCount = existingParentComment.SubCommentsCount + 1,
                        UpdatedAt = _dateTimeProvider.UtcNow
                    }
                );
            }
        }

        _ticketRepository.Save(
            existingTicket with
            {
                CommentsCount = existingTicket.CommentsCount + 1,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );
    }
}
