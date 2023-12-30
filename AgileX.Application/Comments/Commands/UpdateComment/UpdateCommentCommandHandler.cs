using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommandHandler
    : IRequestHandler<UpdateCommentCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateCommentCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ICommentRepository commentRepository,
        ITicketRepository ticketRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _commentRepository = commentRepository;
        _ticketRepository = ticketRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        UpdateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
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

        _commentRepository.Save(
            existingComment with
            {
                Text = request.Text ?? existingComment.Text,
                AttachedCode = request.AttachedCode ?? existingComment.AttachedCode,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );

        return new SuccessMessage("Comment updated successfully");
    }
}
