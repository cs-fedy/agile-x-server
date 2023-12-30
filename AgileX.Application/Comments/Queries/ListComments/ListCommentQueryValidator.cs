using FluentValidation;

namespace AgileX.Application.Comments.Queries.ListComments;

public class ListCommentQueryValidator : AbstractValidator<ListCommentsQuery>
{
    public ListCommentQueryValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
