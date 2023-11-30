using AgileX.Application.Authentication.Commands.Register;
using AgileX.Application.Authentication.Queries.Login;
using AgileX.Application.Common.Result;
using AgileX.Contracts.Authentication.Login;
using AgileX.Contracts.Authentication.Register;
using AgileX.Domain.Common.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgileX.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthenticationController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.Email,
            request.Password,
            request.Username,
            request.FullName
        );

        Result<SuccessMessage> result = await _mediator.Send(command);

        return result.Match(
            res => Ok(res),
            err =>
            {
                var details = new ProblemDetails();

                var reason = new Dictionary<string, object>()
                {
                    { "description", err.Description },
                    { "code", err.Code },
                    { "type", err.Type }
                };

                details.Extensions.Add("isOk", false);
                details.Extensions.Add("errors", reason);

                return new ObjectResult(details);
            }
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);
        Result<string> result = await _mediator.Send(query);

        return result.Match(
            res => Ok(res),
            err =>
            {
                var details = new ProblemDetails();

                var reason = new Dictionary<string, object>()
                {
                    { "description", err.Description },
                    { "code", err.Code },
                    { "type", err.Type }
                };

                details.Extensions.Add("isOk", false);
                details.Extensions.Add("errors", reason);

                return new ObjectResult(details);
            }
        );
    }
}
