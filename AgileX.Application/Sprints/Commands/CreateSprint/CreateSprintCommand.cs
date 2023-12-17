using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Commands.CreateSprint;

public abstract record CreateSprintCommand(
    Guid ProjectId,
    Guid UserId,
    string Name,
    string Description,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<Result<SuccessMessage>>;
