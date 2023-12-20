using FluentValidation;

namespace AgileX.Application.Tickets.Commands.ChangeTicketStatus;

public class ChangeTicketStatusCommandValidator : AbstractValidator<ChangeTicketStatusCommand>
{
    public ChangeTicketStatusCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.CompletionStatus).NotEmpty();
    }
}
