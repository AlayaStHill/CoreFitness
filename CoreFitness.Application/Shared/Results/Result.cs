
namespace CoreFitness.Application.Shared.Results;

public sealed record class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ResultError? Error { get; }

    private Result(bool isSuccess, ResultError? error)
    {
        if (isSuccess && error is not null)
            throw new ArgumentException("Success result cannot have an error.");

        if (!isSuccess && error is null)
            throw new ArgumentException("Failure result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(isSuccess: true, error: null);

    public static Result Fail(ResultError error) => new(isSuccess: false, error: error);

    // overload
    public static Result Fail(ErrorTypes type, string message)
    => new(isSuccess: false, error: new ResultError(type, message));
}

public sealed record class Result<T>
{
    private readonly T? _value;
    public T Value
    {
        get
        {
            if (!IsSuccess)
                throw new InvalidOperationException("Cannot access data of a failed result.");
            return _value!;
        }
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ResultError? Error { get; }

    private Result(bool isSuccess, T? value, ResultError? error)
    {
        if (isSuccess && error is not null)
            throw new ArgumentException("Success result cannot have an error.");

        if (!isSuccess && error is null)
            throw new ArgumentException("Failure result must have an error.");

        if (isSuccess && value is null)
            throw new ArgumentException("Success result must have a value.");

        IsSuccess = isSuccess;
        Error = error;
        _value = value;
    }

    public static Result<T> Success(T value) => new(isSuccess: true, value: value, error: null);

    public static Result<T> Fail(ResultError error) 
        => new(isSuccess: false, value: default, error: error);

    public static Result<T> Fail(ErrorTypes type, string message)
    => new(isSuccess: false, value: default, error: new ResultError(type, message));
}
