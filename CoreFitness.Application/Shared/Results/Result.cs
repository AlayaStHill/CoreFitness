
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
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ResultError? Error { get; }
    public T? Data { get; }
    private Result(bool isSuccess, T? data, ResultError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
        Data = data;
    }

    public static Result<T> Success(T data) => new(isSuccess: true, data: data, error: null);

    public static Result<T> Fail(ResultError error) 
        => new(isSuccess: false, data: default, error: error);

    public static Result<T> Fail(ErrorTypes type, string message)
    => new(isSuccess: false, data: default, error: new ResultError(type, message));
}
