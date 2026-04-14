using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.Tests.Common;

internal static class ResultAssert
{
    public static void Failure(Result result, ResultError expectedError)
    {
        Failure(result, expectedError.Type, expectedError.Message);
    }

    public static void Success(Result result)
    {
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
    }

    public static void Failure(Result result, ErrorTypes expectedType, string? expectedMessage = null)
    {
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(expectedType, result.Error!.Type);

        if (!string.IsNullOrWhiteSpace(expectedMessage))
        {
            Assert.Equal(expectedMessage, result.Error.Message);
        }
    }

    public static void Success<T>(Result<T> result)
    {
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
    }

    public static void Failure<T>(Result<T> result, ResultError expectedError)
    {
        Failure(result, expectedError.Type, expectedError.Message);
    }

    public static void Failure<T>(Result<T> result, ErrorTypes expectedType, string? expectedMessage = null)
    {
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(expectedType, result.Error!.Type);

        if (!string.IsNullOrWhiteSpace(expectedMessage))
        {
            Assert.Equal(expectedMessage, result.Error.Message);
        }
    }
}
