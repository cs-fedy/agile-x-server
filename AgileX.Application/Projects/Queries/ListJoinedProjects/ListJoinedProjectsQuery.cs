using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Projects.Queries.ListJoinedProjects;

public record ListJoinedProjectsQuery(Guid UserId) : IRequest<Result<List<Project>>>;
