using corefitness.domain.shared.validators;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Shared.ValueObjects.PhoneNumbers;

public class PhoneNumber : ValueObject
{
    public const int PhoneNumberMaxLength = 16;
    public const int PhoneNumberMinLength = 8;

    public string Phone { get; }

    private PhoneNumber(string phoneNumber)
    {
        Phone = ValidatePhoneNumber(phoneNumber);
    }

    public static PhoneNumber Create(string phoneNumber) => new(phoneNumber);


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Phone;
    }

    private static string ValidatePhoneNumber(string phoneNumber)
    {
        string normalized = phoneNumber?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(normalized))
            throw new ValidationDomainException(PhoneNumberErrors.PhoneNumberIsRequired);

        if (normalized.Length < PhoneNumberMinLength)
            throw new ValidationDomainException(PhoneNumberErrors.PhoneNumberIsTooShort(PhoneNumberMinLength));

        if (normalized.Length > PhoneNumberMaxLength)
            throw new ValidationDomainException(PhoneNumberErrors.PhoneNumberIsTooLong(PhoneNumberMaxLength));
        return normalized;
    }
}
