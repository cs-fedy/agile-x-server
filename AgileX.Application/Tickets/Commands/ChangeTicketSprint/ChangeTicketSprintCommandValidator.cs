using FluentValidation;

namespace AgileX.Application.Tickets.Commands.ChangeTicketSprint;

public class ChangeTicketSprintCommandValidator : AbstractValidator<ChangeTicketSprintCommand>
{
    public ChangeTicketSprintCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.NewSprintId).NotEmpty();
    }
}
