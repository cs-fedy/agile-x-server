using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Commands.DeleteSprint;

public record DeleteSprintCommand(Guid SprintId, Guid UserId) : IRequest<Result<SuccessMessage>>;
