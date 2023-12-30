using FluentValidation;

namespace AgileX.Application.Comments.Queries.ListSubComments;

public class ListSubCommentsQueryValidator : AbstractValidator<ListSubCommentsQuery>
{
    public ListSubCommentsQueryValidator()
    {
        RuleFor(x => x.ParentCommentId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
