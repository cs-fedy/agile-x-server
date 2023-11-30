using AgileX.Application.Common.Result;
using AgileX.Domain.Common.Result;
using MediatR;

namespace AgileX.Application.Authentication.Commands.Register;

public record RegisterCommand(string Email, string Password, string Username, string FullName)
    : IRequest<Result<SuccessMessage>>;
