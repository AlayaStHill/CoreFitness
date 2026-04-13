using corefitness.domain.shared.validators;
using CoreFitness.Domain.Tests.Common;

namespace CoreFitness.Domain.Tests.Shared.Validators;

public sealed class DomainValidatorTests
{
    [Fact]
    public void RequiredString_ShouldThrow_WhenValueIsMissing()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => DomainValidator.RequiredString(" ", "Required"),
            "Required");
    }

    [Fact]
    public void RequiredString_ShouldTrimValue_WhenInputHasWhitespace()
    {
        var value = DomainValidator.RequiredString("  abc  ", "Required");

        Assert.Equal("abc", value);
    }

    [Fact]
    public void RequiredGuid_ShouldThrow_WhenGuidIsEmpty()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => DomainValidator.RequiredGuid(Guid.Empty, "Guid required"),
            "Guid required");
    }

    [Fact]
    public void RequiredWithLength_ShouldThrow_WhenValueTooShort()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => DomainValidator.RequiredWithLength("ab", 3, 10, "required", "short", "long"),
            "short");
    }

    [Fact]
    public void RequiredWithLength_ShouldThrow_WhenValueTooLong()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => DomainValidator.RequiredWithLength("abcdefghijk", 3, 10, "required", "short", "long"),
            "long");
    }

    [Fact]
    public void Optional_ShouldReturnTrimmedValue_WhenInputHasWhitespace()
    {
        var value = DomainValidator.Optional("  abc  ");

        Assert.Equal("abc", value);
    }

    [Fact]
    public void Optional_ShouldReturnNull_WhenInputIsWhitespace()
    {
        var value = DomainValidator.Optional("   ");

        Assert.Null(value);
    }
}
