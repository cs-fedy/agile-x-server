namespace AgileX.Infrastructure.Settings;

public class SendGridSettings
{
    public const string SectionName = "SendGridSettings";
    public string Key { get; init; } = string.Empty;
    public string Sender { get; init; } = string.Empty;
}
