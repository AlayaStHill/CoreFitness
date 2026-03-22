using corefitness.domain.shared.validators;
using CoreFitness.Domain.Shared.ValueObjects.EmailAddresses;
using CoreFitness.Domain.Shared.ValueObjects.PhoneNumbers;

namespace CoreFitness.Domain.Aggregates.CustomerService;

public sealed class ContactRequest
{
    public const int NameMaxLength = 20;
    public const int NameMinLength = 2;
    public const int MessageMaxLength = 2000;
    public const int MessageMinLength = 5;

    public string Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public EmailAddress Email { get; }
    public PhoneNumber? PhoneNumber { get; }
    public string Message { get; }
    public DateTimeOffset CreatedAt { get; }
    public bool MarkedAsRead { get; private set; }

    private ContactRequest(string id, string firstName, string lastName, EmailAddress email, PhoneNumber? phoneNumber, string message, DateTimeOffset createdAt, bool markedAsRead)
    {
        Id = id;
        FirstName = DomainValidator.RequiredWithLength(firstName, NameMinLength, NameMaxLength, ContactRequestErrors.FirstNameRequired, ContactRequestErrors.FirstNameTooShort(NameMinLength), ContactRequestErrors.FirstNameTooLong(NameMaxLength));
        LastName = DomainValidator.RequiredWithLength(lastName, NameMinLength, NameMaxLength, ContactRequestErrors.LastNameRequired, ContactRequestErrors.LastNameTooShort(NameMinLength), ContactRequestErrors.LastNameTooLong(NameMaxLength));
        Email = email;
        PhoneNumber = phoneNumber;
        Message = DomainValidator.RequiredWithLength(message, MessageMinLength, MessageMaxLength, ContactRequestErrors.MessageRequired, ContactRequestErrors.MessageTooShort(MessageMinLength), ContactRequestErrors.MessageTooLong(MessageMaxLength));
        CreatedAt = createdAt;
        MarkedAsRead = markedAsRead;
    }

    public static ContactRequest Create(string firstName, string lastName, string email, string? phoneNumber, string message)
        => new(Guid.NewGuid().ToString(), firstName, lastName, EmailAddress.Create(email), string.IsNullOrWhiteSpace(phoneNumber) ? null : PhoneNumber.Create(phoneNumber), message, DateTimeOffset.UtcNow, false);

    public static ContactRequest Rehydrate(string id, string firstName, string lastName, EmailAddress email, PhoneNumber? phoneNumber, string message, DateTimeOffset createdAt, bool markedAsRead)
        => new(id, firstName, lastName, email, phoneNumber, message, createdAt, markedAsRead);

    public void MarkAsRead()
    {
        if (MarkedAsRead)
            return;

        MarkedAsRead = true;
    }

    public void MarkAsUnread()
    {
        if (!MarkedAsRead)
            return;

        MarkedAsRead = false;
    }
}
