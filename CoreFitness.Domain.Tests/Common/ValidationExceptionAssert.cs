using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Tests.Common;

internal static class ValidationExceptionAssert
{
    public static void ThrowsWithMessage(Action action, string expectedMessage)
    {
        var exception = Assert.Throws<ValidationDomainException>(action);
        Assert.Equal(expectedMessage, exception.Message);
    }
}
