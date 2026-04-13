using CoreFitness.Domain.Aggregates.CustomerService;
using CoreFitness.Domain.Tests.Common;

namespace CoreFitness.Domain.Tests.Aggregates.CustomerService;

public sealed class ContactRequestTests
{
    [Fact]
    public void Create_ShouldThrow_WhenFirstNameIsMissing()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => ContactRequest.Create(" ", "Doe", "john@doe.com", "0701234567", "Hej där"),
            ContactRequestErrors.FirstNameRequired);
    }

    [Fact]
    public void Create_ShouldThrow_WhenMessageIsTooShort()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => ContactRequest.Create("John", "Doe", "john@doe.com", "0701234567", "Hej"),
            ContactRequestErrors.MessageTooShort(ContactRequest.MessageMinLength));
    }

    [Fact]
    public void Create_ShouldAllowMissingPhoneNumber()
    {
        var request = ContactRequest.Create("John", "Doe", "john@doe.com", null, "Hej detta är ett meddelande");

        Assert.Null(request.PhoneNumber);
    }

    [Fact]
    public void MarkAsRead_And_MarkAsUnread_ShouldToggleReadState()
    {
        var request = ContactRequest.Create("John", "Doe", "john@doe.com", "0701234567", "Hej detta är ett meddelande");

        request.MarkAsRead();
        Assert.True(request.MarkedAsRead);

        request.MarkAsUnread();
        Assert.False(request.MarkedAsRead);
    }

    [Fact]
    public void MarkAsRead_ShouldRemainRead_WhenCalledTwice()
    {
        var request = ContactRequest.Create("John", "Doe", "john@doe.com", "0701234567", "Hej detta är ett meddelande");

        request.MarkAsRead();
        request.MarkAsRead();

        Assert.True(request.MarkedAsRead);
    }

    [Fact]
    public void MarkAsUnread_ShouldRemainUnread_WhenCalledTwice()
    {
        var request = ContactRequest.Create("John", "Doe", "john@doe.com", "0701234567", "Hej detta är ett meddelande");

        request.MarkAsUnread();
        request.MarkAsUnread();

        Assert.False(request.MarkedAsRead);
    }
}
