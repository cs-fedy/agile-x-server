using FluentValidation;

namespace AgileX.Application.Comments.Queries.GetComment;

public class GetCommentQueryValidator : AbstractValidator<GetCommentQuery>
{
    public GetCommentQueryValidator()
    {
        RuleFor(x => x.CommentId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
