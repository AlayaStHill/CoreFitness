namespace CoreFitness.Application.Shared.Results;

public sealed record class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ResultError? Error { get; }

    private Result(bool isSuccess, ResultError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(isSuccess: true, error: null);

    public static Result Fail(ResultError error) => new(isSuccess: false, error: error);
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
}
