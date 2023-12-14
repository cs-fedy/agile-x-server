namespace AgileX.Infrastructure.Settings;

public class MessageBrokerSettings
{
    public const string SectionName = "MessageBrokerSettings";
    public string Host { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
