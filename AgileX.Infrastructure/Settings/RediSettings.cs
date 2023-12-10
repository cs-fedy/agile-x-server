namespace AgileX.Infrastructure.Settings;

public class RedisSettings
{
    public const string SectionName = "RedisSettings";
    public string Host { get; init; }
    public int Port { get; init; }
}
