using FluentValidation;

namespace AgileX.Application.Tickets.Commands.CreateTicket;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Deadline).NotEmpty();
        RuleFor(x => x.Priority).NotEmpty();
        RuleFor(x => x.Reminder).NotEmpty();
    }
}
