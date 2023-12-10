using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Domain.ObjectValues;
using AgileX.Infrastructure.Services;
using AgileX.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace AgileX.Infrastructure.Authentication;

public class RefreshGenerator : IRefreshGenerator
{
    private readonly DateTimeProvider _dateTimeProvider;
    private readonly RefreshSettings _refreshSettings;

    public RefreshGenerator(
        DateTimeProvider dateTimeProvider,
        IOptions<RefreshSettings> refreshOptions
    )
    {
        _dateTimeProvider = dateTimeProvider;
        _refreshSettings = refreshOptions.Value;
    }

    public RefreshToken Generate() =>
        new RefreshToken(
            Guid.NewGuid(),
            _dateTimeProvider.UtcNow.AddDays(_refreshSettings.ExpiryDays)
        );
}
