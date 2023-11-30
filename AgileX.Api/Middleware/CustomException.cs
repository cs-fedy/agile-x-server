using System.Diagnostics.CodeAnalysis;

namespace AgileX.Api.Middleware;

public class CustomException : Exception
{
    public string? Type { get; }
    public object Reason { get; }

    public CustomException([NotNull] string message, object reason, string? type = null)
        : base(message)
    {
        Reason ??= reason;
        Type = type;
    }
}
