using CoreFitness.Domain.Shared.ValueObjects.PhoneNumbers;
using CoreFitness.Domain.Tests.Common;

namespace CoreFitness.Domain.Tests.Shared.ValueObjects.PhoneNumbers;

public sealed class PhoneNumberTests
{
    [Fact]
    public void Create_ShouldThrow_WhenPhoneNumberIsMissing()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => PhoneNumber.Create(" "),
            PhoneNumberErrors.PhoneNumberIsRequired);
    }

    [Fact]
    public void Create_ShouldThrow_WhenPhoneNumberIsTooShort()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => PhoneNumber.Create(new string('1', PhoneNumber.PhoneNumberMinLength - 1)),
            PhoneNumberErrors.PhoneNumberIsTooShort(PhoneNumber.PhoneNumberMinLength));
    }

    [Fact]
    public void Create_ShouldThrow_WhenPhoneNumberIsTooLong()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => PhoneNumber.Create(new string('1', PhoneNumber.PhoneNumberMaxLength + 1)),
            PhoneNumberErrors.PhoneNumberIsTooLong(PhoneNumber.PhoneNumberMaxLength));
    }

    [Fact]
    public void Create_ShouldTrimPhoneNumber_WhenInputHasWhitespace()
    {
        var phoneNumber = PhoneNumber.Create("  0701234567  ");

        Assert.Equal("0701234567", phoneNumber.Phone);
    }
}
