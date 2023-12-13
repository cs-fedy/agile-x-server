using System.Text;
using AgileX.Application.Common;
using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Infrastructure.Authentication;
using AgileX.Infrastructure.Cache;
using AgileX.Infrastructure.Persistence;
using AgileX.Infrastructure.Persistence.Repositories;
using AgileX.Infrastructure.Services;
using AgileX.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Helpers.Mail;

namespace AgileX.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.AddAuth(configuration);
        services.AddCache(configuration);
        services.AddDatabase(configuration);
        services.AddProviders(configuration);
        services.AddRepositories(configuration);

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    private static IServiceCollection AddProviders(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        SendGridSettings settings = new();
        configuration.Bind(SendGridSettings.SectionName, settings);

        services.AddSingleton<IEmailProvider>(
            new SendGridProvider(
                new SendGrid.SendGridClient(settings.Key),
                new EmailAddress(settings.Sender)
            )
        );

        services.AddScoped<IEventProvider, EventProvider>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPasswordProvider, PasswordProvider>();
        services.AddSingleton<ICodeProvider, CodeProvider>();

        return services;
    }

    private static IServiceCollection AddCache(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        RedisSettings settings = new();
        configuration.Bind(RedisSettings.SectionName, settings);

        var redisClient = new RedisCacheContext(Options.Create(settings));
        services.AddSingleton(redisClient.RedisDB);

        services.AddScoped<ICacheRepository, CacheRepository>();

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        DatabaseSettings settings = new();
        configuration.Bind(DatabaseSettings.SectionName, settings);

        services.AddDbContext<CustomDbContext>(options => options.UseNpgsql(settings.PgCnxString));
        return services;
    }

    private static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        JwtSettings settings = new();
        configuration.Bind(JwtSettings.SectionName, settings);

        services.AddSingleton(Options.Create(settings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IRefreshGenerator, RefreshGenerator>();

        services
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                options =>
                    options.TokenValidationParameters = new TokenValidationParameters
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
