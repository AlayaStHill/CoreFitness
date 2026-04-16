namespace CoreFitness.Domain.Shared.ValueObjects.EmailAddresses;

public class EmailAddressErrors
{
    public const string EmailIsRequired = "Email is required.";
    public static string EmailIsTooLong(int maxLength) => $"Email cannot be longer than {maxLength} characters.";
}
