using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgileX.Api.Controllers;

[ApiController]
[Authorize]
[Route("users")]
public class UsersController : ControllerBase
{
    [HttpGet("logged")]
    public async Task<IActionResult> GetLoggedUser()
    {
        await Task.CompletedTask;
        return Ok();
    }
}
