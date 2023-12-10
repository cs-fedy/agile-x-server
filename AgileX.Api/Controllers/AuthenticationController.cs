using AgileX.Application.Authentication.Commands.Register;
using AgileX.Application.Authentication.Queries.Login;
using AgileX.Contracts.Authentication.Login;
using AgileX.Contracts.Authentication.Register;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgileX.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        var result = await _mediator.Send(command);

        return result.Match(
            res => Ok(_mapper.Map<RegisterResponse>(res)),
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
        var query = _mapper.Map<LoginQuery>(request);
        var result = await _mediator.Send(query);

        return result.Match(
            res =>
            {
                Response
                    .Cookies
                    .Append(
                        "refresh-token",
                        res.RefreshToken.token.ToString(),
                        new CookieOptions()
                        {
                            HttpOnly = true,
                            Domain = HttpContext.Request.Host.ToString(),
                            Path = HttpContext.Request.Path,
                            SameSite = SameSiteMode.None,
                            Secure = HttpContext.Request.IsHttps
                        }
                    );

                return Ok(_mapper.Map<LoginResponse>(res));
            },
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
