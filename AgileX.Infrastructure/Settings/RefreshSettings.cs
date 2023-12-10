namespace AgileX.Infrastructure.Settings;

public class RefreshSettings
{
    public const string SectionName = "RefreshSettings";
    public int ExpiryDays { get; init; }
}
