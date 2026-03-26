using CoreFitness.Domain.Exceptions.Custom;

namespace corefitness.domain.shared.validators;

public static class DomainValidator
{
    public static string RequiredString(string? value, string errorMessage)
    {
        string normalizedValue = value?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(normalizedValue))
            throw new ValidationDomainException(errorMessage);

        return normalizedValue;
    }

    public static void RequiredGuid(Guid value, string errorMessage)
    {
        if (value == Guid.Empty)
        {
            throw new ValidationDomainException(errorMessage);
        }
    }

    public static string RequiredWithLength(string? value, int minLength, int maxLength, string requiredErrorMessage, string tooShortErrorMessage, string tooLongErrorMessage)
    {
        string normalizedValue = value?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(normalizedValue))
            throw new ValidationDomainException(requiredErrorMessage);

        if (normalizedValue.Length < minLength)
            throw new ValidationDomainException(tooShortErrorMessage);

        if (normalizedValue.Length > maxLength)
            throw new ValidationDomainException(tooLongErrorMessage);

        return normalizedValue;
    }

    public static string? Optional(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value.Trim();
    }

    internal static void RequiredGuid(Guid value, object workoutTypeIdRequired)
    {
        throw new NotImplementedException();
    }

    internal static void RequiredString(string identityUserId, object identityUserIdRequired)
    {
        throw new NotImplementedException();
    }
}