using FluentValidation;

namespace AgileX.Application.Tickets.Queries.ListSprintTickets;

public class ListSprintTicketsQueryValidator : AbstractValidator<ListSprintTicketsQuery>
{
    public ListSprintTicketsQueryValidator()
    {
        RuleFor(x => x.SprintId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
