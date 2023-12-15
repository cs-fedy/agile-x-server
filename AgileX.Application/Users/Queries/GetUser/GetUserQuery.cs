using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Users.Queries.GetUser;

public record GetUserQuery(Guid UserId) : IRequest<Result<User>>;
