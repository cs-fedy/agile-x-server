using System.Security.Claims;
using AgileX.Api.Common.Errors;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AgileX.Api.Attributes;

public class LoggedUserAttribute : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var claims = context.HttpContext.User.Claims;
        string userId = claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        if (userId is null)
            throw new CustomException(
                message: "Invalid / not found user id",
                reason: Error.NotFound(),
                status: 404
            );
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
