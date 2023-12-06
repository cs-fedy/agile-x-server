using System.Text;
using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Infrastructure.Authentication;
using AgileX.Infrastructure.Persistence;
using AgileX.Infrastructure.Services;
using AgileX.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgileX.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.AddAuth(configuration);
        services.AddDatabase(configuration);
        services.AddProviders(configuration);
        services.AddRepositories(configuration);

        return services;
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    public static IServiceCollection AddProviders(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        SendGridSettings settings = new();
        configuration.Bind(SendGridSettings.SectionName, settings);
        services.AddSingleton(new SendGrid.SendGridClient(settings.Key));

        services.AddSingleton<IEmailProvider, SendGridProvider>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPasswordProvider, PasswordProvider>();
        return services;
    }

    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        DatabaseSettings settings = new();
        configuration.Bind(DatabaseSettings.SectionName, settings);

        services.AddDbContext<CustomDbContext>(options => options.UseNpgsql(settings.PgCnxString));
        return services;
    }

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        JwtSettings settings = new();
        configuration.Bind(JwtSettings.SectionName, settings);

        services.AddSingleton(Options.Create(settings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                options =>
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = settings.Issuer,
                        ValidAudience = settings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(settings.Secret)
                        )
                    }
            );

        return services;
    }
}
