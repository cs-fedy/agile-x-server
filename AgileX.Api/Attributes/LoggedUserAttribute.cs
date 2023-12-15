using System.Security.Claims;
using AgileX.Api.Common.Errors;
using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Result;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AgileX.Api.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class LoggedUserAttribute : Attribute, IActionFilter
{
    private readonly IUserRepository _userRepository;

    public LoggedUserAttribute(IUserRepository userRepository) => _userRepository = userRepository;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var claims = context.HttpContext.User.Claims;
        var userIdClaim = claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        if (userIdClaim is null)
            throw new CustomException(
                message: "Invalid / not found user",
                reason: Error.NotFound(),
                status: 404
            );

        var userId = new Guid(userIdClaim);
        var existingUser = _userRepository.GetById(userId);

        if (existingUser is null || existingUser.IsDeleted)
            throw new CustomException(
                message: "Invalid / not found user",
                reason: Error.NotFound(),
                status: 404
            );
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
