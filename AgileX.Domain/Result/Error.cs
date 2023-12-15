namespace AgileX.Domain.Result;

public struct Error
{
    public string Code { get; }
    public string Description { get; set; }
    public ErrorType Type { get; }
    public int StatusCode { get; }

    private Error(string code, string description, ErrorType type, int statusCode)
    {
        Code = code;
        Description = description;
        Type = type;
        StatusCode = statusCode;
    }

    public static Error Failure(
        string code = "General.Failure",
        string description = "A failure has occurred.",
        int statusCode = 500
    ) => new(code, description, ErrorType.Failure, statusCode);

    public static Error Unexpected(
        string code = "General.Unexpected",
        string description = "An unexpected error has occurred.",
        int statusCode = 500
    ) => new(code, description, ErrorType.Unexpected, statusCode);

    public static Error Validation(
        string code = "General.Validation",
        string description = "A validation error has occurred.",
        int statusCode = 400
    ) => new(code, description, ErrorType.Validation, statusCode);

    public static Error Conflict(
        string code = "General.Conflict",
        string description = "A conflict error has occurred.",
        int statusCode = 409
    ) => new(code, description, ErrorType.Conflict, statusCode);

    public static Error NotFound(
        string code = "General.NotFound",
        string description = "A 'Not Found' error has occurred.",
        int statusCode = 404
    ) => new(code, description, ErrorType.NotFound, statusCode);

    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string description = "An 'Unauthorized' error has occurred.",
        int statusCode = 401
    ) => new(code, description, ErrorType.Unauthorized, statusCode);

    public static Error Custom(int type, string code, string description, int statusCode) =>
        new(code, description, (ErrorType)type, statusCode);
}
