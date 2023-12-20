using AgileX.Application.Tickets.Queries.ListSubTickets;
using FluentValidation;

namespace AgileX.Application.Tickets.Queries.GetTicket;

public class GetTicketQueryValidator : AbstractValidator<GetTicketQuery>
{
    public GetTicketQueryValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
