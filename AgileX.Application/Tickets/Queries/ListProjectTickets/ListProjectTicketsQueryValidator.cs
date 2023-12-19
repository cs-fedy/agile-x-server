using FluentValidation;

namespace AgileX.Application.Tickets.Queries.ListProjectTickets;

public class ListProjectTicketsQueryValidator : AbstractValidator<ListProjectTicketsQuery>
{
    public ListProjectTicketsQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
