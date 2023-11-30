using AgileX.Application.Common.Interfaces.Services;

namespace AgileX.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
