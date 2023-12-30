using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Comments.Queries.GetComment;

public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, Result<Comment>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ITicketRepository _ticketRepository;

    public GetCommentQueryHandler(
        ICommentRepository commentRepository,
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ITicketRepository ticketRepository
    )
    {
        _commentRepository = commentRepository;
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<Comment>> Handle(
        GetCommentQuery request,
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

        return existingComment;
    }
}
