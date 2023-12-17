using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Commands.UpdateSprint;

public abstract record UpdateSprintCommand(
    Guid SprintId,
    Guid UserId,
    string? Name,
    string? Description
) : IRequest<Result<SuccessMessage>>;
