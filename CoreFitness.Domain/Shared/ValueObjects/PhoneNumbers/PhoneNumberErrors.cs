namespace CoreFitness.Domain.Shared.ValueObjects.PhoneNumbers;

public static class PhoneNumberErrors
{
    public const string PhoneNumberIsRequired = "Phone number is required.";
    public static string PhoneNumberIsTooShort(int minLength) => $"Phone number must be at least {minLength} characters long.";
    public static string PhoneNumberIsTooLong(int maxLength) => $"Phone number must be at most {maxLength} characters long.";
}