using corefitness.domain.shared.validators;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Shared.ValueObjects.EmailAddresses;

public class EmailAddress : ValueObject
{
    public const int EmailMaxLength = 254;

    public string Email { get; }

    private EmailAddress(string email)
    {
        Email = ValidateEmail(email);
    }

    public static EmailAddress Create(string email) => new(email);  


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Email;
    }

    private static string ValidateEmail(string email)
    {
        string normalized = email?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(normalized))
            throw new ValidationDomainException(EmailAddressErrors.EmailIsRequired);

        if (normalized.Length > EmailMaxLength)
            throw new ValidationDomainException(EmailAddressErrors.EmailIsTooLong(EmailMaxLength));

        return normalized;
    }
}

