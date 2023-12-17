using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Projects.Queries.GetProject;

public abstract record GetProjectQuery(Guid ProjectId, Guid UserId) : IRequest<Result<Project>>;
