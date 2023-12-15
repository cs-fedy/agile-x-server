using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Projects.Commands.CreateProject;

public abstract record CreateProjectCommand(
    Guid UserId,
    string Name,
    string Description,
    int Priority,
    DateTime Deadline
) : IRequest<Result<SuccessMessage>>;
