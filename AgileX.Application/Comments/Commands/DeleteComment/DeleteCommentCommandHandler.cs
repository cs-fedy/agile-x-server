using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Comments.Commands.DeleteComment;

public class DeleteCommentCommandHandler
    : IRequestHandler<DeleteCommentCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IEventProvider _eventProvider;

    public DeleteCommentCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ICommentRepository commentRepository,
        ITicketRepository ticketRepository,
        IEventProvider eventProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _commentRepository = commentRepository;
        _ticketRepository = ticketRepository;
        _eventProvider = eventProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingComment = _commentRepository.GetById(request.CommentId);
        if (existingComment is null || existingComment.IsDeleted)
            return CommentErrors.CommentNotFound;

        var existingTicket = _ticketRepository.GetById(existingComment.TicketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return TicketErrors.TicketNotFound;

        var existingProject = _projectRepository.GetById(existingTicket.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMember = _memberRepository.Get(existingProject.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        if (existingComment.CommentedBy != request.UserId)
            return CommentErrors.NotCommentOwner;

        _commentRepository.Delete(request.CommentId);

        await _eventProvider.Publish(new CommentDeleted(CommentId: request.CommentId));
        return new SuccessMessage("Comment deleted successfully");
    }
}
