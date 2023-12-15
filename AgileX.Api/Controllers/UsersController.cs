using System.Security.Claims;
using AgileX.Api.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgileX.Api.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UsersController : ControllerBase
{
    [HttpGet("logged")]
    public async Task<IActionResult> GetLoggedUser()
    {
        await Task.CompletedTask;
        return Ok();
    }
}
