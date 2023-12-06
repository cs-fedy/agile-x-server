namespace AgileX.Domain.Result;

public class Result<T>
{
    private readonly T? _value = default;
    private readonly Error? _error = null;

    public bool IsOk { get; }

    public Error Failure =>
        !IsOk ? (Error)_error! : throw new Exception("You can't access errors when IsOk is true");

    public T Success =>
        IsOk ? _value! : throw new Exception("You can't access data when IsOk is false");

    private Result(Error error)
    {
        _error = error;
        IsOk = false;
    }

    public static Result<T> From(Error error) => error;

    public static implicit operator Result<T>(Error error) => new Result<T>(error);

    private Result(T value)
    {
        _value = value;
        IsOk = true;
    }

    public static Result<T> From(T value) => value;

    public static implicit operator Result<T>(T value) => new Result<T>(value);

    public TR Match<TR>(Func<T, TR> onValue, Func<Error, TR> onError) =>
        IsOk ? onValue(Success) : onError(Failure);

    public async Task<TR> MatchAsync<TR>(Func<T, Task<TR>> onValue, Func<Error, Task<TR>> onError)
    {
        return IsOk ? await onValue(Success) : await onError(Failure);
    }
}
