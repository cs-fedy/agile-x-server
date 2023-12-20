using FluentValidation;

namespace AgileX.Application.Tickets.Commands.ChangeTicketDeadline;

public class ChangeTicketDeadlineCommandValidator : AbstractValidator<ChangeTicketDeadlineCommand>
{
    public ChangeTicketDeadlineCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NewDeadline).NotEmpty();
    }
}
