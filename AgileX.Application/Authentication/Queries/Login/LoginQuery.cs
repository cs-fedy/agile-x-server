using AgileX.Application.Common.Result;
using MediatR;

namespace AgileX.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<Result<LoginResul>>;
