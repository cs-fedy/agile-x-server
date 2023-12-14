namespace AgileX.Infrastructure.Settings;

public class DatabaseSettings
{
    public const string SectionName = "DatabaseSettings";
    public string PgCnxString { get; init; } = string.Empty;
}
