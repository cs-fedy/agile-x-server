using AgileX.Api.Common.Errors;
using AgileX.Api.Common.Mapping;
using AgileX.Api.Middleware;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AgileX.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();

        services
            .AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>()
            .AddSingleton<ErrorHandlingMiddleware>();

        services.AddMappings();
        return services;
    }
}
