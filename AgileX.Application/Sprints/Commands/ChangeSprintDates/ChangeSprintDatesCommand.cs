using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Commands.ChangeSprintDates;

public record ChangeSprintDatesCommand(
    Guid SprintId,
    Guid UserId,
    DateTime? NewStartDate,
    DateTime? NewEndDate
) : IRequest<Result<SuccessMessage>>;
