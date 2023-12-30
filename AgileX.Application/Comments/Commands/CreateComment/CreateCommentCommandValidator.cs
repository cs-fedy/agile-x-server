using FluentValidation;

namespace AgileX.Application.Comments.Commands.CreateComment;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Text).NotEmpty();
        RuleFor(x => x.AttachedCode).NotEmpty();
    }
}
