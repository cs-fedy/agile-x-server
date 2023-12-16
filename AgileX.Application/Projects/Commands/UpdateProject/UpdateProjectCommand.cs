using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Projects.Commands.UpdateProject;

public abstract record UpdateProjectCommand(
    Guid ProjectId,
    Guid UserId,
    string? Name,
    string? Description,
    int? Priority
) : IRequest<Result<SuccessMessage>>;
