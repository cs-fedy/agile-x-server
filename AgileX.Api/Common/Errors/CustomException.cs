using System.Diagnostics.CodeAnalysis;

namespace AgileX.Api.Common.Errors;

public class CustomException : Exception
{
    public string? Type { get; }
    public object? Reason { get; }
    public int? Status { get; }

    public CustomException(
        [NotNull] string message,
        object? reason = null,
        string? type = null,
        int? status = null
    )
        : base(message)
    {
        Reason = reason;
        Type = type;
        Status = status;
    }
}
