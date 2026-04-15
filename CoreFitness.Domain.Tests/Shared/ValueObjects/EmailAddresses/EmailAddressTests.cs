using CoreFitness.Domain.Shared.ValueObjects.EmailAddresses;
using CoreFitness.Domain.Tests.Common;

namespace CoreFitness.Domain.Tests.Shared.ValueObjects.EmailAddresses;

public sealed class EmailAddressTests
{
    [Fact]
    public void Create_ShouldThrow_WhenEmailIsMissing()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => EmailAddress.Create(" "),
            EmailAddressErrors.EmailIsRequired);
    }

    [Fact]
    public void Create_ShouldThrow_WhenEmailIsTooLong()
    {
        var tooLongEmail = new string('a', EmailAddress.EmailMaxLength + 1);

        ValidationExceptionAssert.ThrowsWithMessage(
            () => EmailAddress.Create(tooLongEmail),
            EmailAddressErrors.EmailIsTooLong(EmailAddress.EmailMaxLength));
    }

    [Fact]
    public void Create_ShouldTrimEmail_WhenInputHasWhitespace()
    {
        var email = EmailAddress.Create("  john@doe.com  ");

        Assert.Equal("john@doe.com", email.Email);
    }
}
