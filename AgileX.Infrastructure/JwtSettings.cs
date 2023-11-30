namespace AgileX.Infrastructure;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public required string Secret { get; init; }
    public required string Issuer { get; init; }
    public int ExpiryMinutes { get; init; }
}
