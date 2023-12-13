using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.ObjectValues;
using AgileX.Infrastructure.Services;
using AgileX.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace AgileX.Infrastructure.Authentication;

public class RefreshGenerator : IRefreshGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly RefreshSettings _refreshSettings;

    public RefreshGenerator(
        IDateTimeProvider dateTimeProvider,
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
