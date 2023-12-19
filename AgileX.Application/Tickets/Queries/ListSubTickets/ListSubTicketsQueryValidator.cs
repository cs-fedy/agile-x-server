using FluentValidation;

namespace AgileX.Application.Tickets.Queries.ListSubTickets;

public class ListSubTicketsQueryValidator : AbstractValidator<ListSubTicketsQuery>
{
    public ListSubTicketsQueryValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
