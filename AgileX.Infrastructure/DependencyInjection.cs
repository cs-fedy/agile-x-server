using System.Reflection;
using System.Text;
using AgileX.Application.Common.Events;
using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Infrastructure.Authentication;
using AgileX.Infrastructure.Cache;
using AgileX.Infrastructure.MessageBroker;
using AgileX.Infrastructure.Persistence;
using AgileX.Infrastructure.Persistence.Repositories;
using AgileX.Infrastructure.Services;
using AgileX.Infrastructure.Settings;
using MassTransit;
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
        services.AddMessageQueue(configuration);

        return services;
    }

    private static void AddMessageQueue(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        MessageBrokerSettings settings = new();
        configuration.Bind(MessageBrokerSettings.SectionName, settings);

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            // TODO: fix unroutable messages
            busConfigurator.AddConsumers(Assembly.GetEntryAssembly());

            busConfigurator.UsingRabbitMq(
                (context, configurator) =>
                {
                    configurator.Host(
                        new Uri(settings.Host),
                        host =>
                        {
                            host.Username(settings.Username);
                            host.Password(settings.Password);
                        }
                    );
                }
            );
        });

        services.AddTransient<IEventBus, EventBus>();
    }

    private static void AddRepositories(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshRepository, RefreshRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IMemberPermissionRepository, MemberPermissionRepository>();
        services.AddScoped<ISprintRepository, SprintRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<IDependencyRepository, DependencyRepository>();
        services.AddScoped<ILabelRepository, LabelRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
    }

    private static void AddProviders(this IServiceCollection services, IConfiguration configuration)
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
    }

    private static void AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        RedisSettings settings = new();
        configuration.Bind(RedisSettings.SectionName, settings);

        var redisClient = new RedisCacheContext(Options.Create(settings));
        services.AddSingleton(redisClient.RedisDB);

        services.AddScoped<ICacheRepository, CacheRepository>();
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseSettings settings = new();
        configuration.Bind(DatabaseSettings.SectionName, settings);
        services.AddDbContext<CustomDbContext>(options => options.UseNpgsql(settings.PgCnxString));
    }

    private static void AddAuth(this IServiceCollection services, IConfiguration configuration)
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
    }
}
