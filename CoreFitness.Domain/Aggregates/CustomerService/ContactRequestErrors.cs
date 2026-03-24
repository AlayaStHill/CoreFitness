namespace CoreFitness.Domain.Aggregates.CustomerService;

public static class ContactRequestErrors
{
    public const string FirstNameRequired = "First name is required.";
    public static string FirstNameTooShort(int minLength) => $"First name must be at least {minLength} characters.";
    public static string FirstNameTooLong(int maxLength) => $"First name must be at most {maxLength} characters.";

    public const string LastNameRequired = "Last name is required.";
    public static string LastNameTooShort(int minLength) => $"Last name must be at least {minLength} characters.";
    public static string LastNameTooLong(int maxLength) => $"Last name must be at most {maxLength} characters.";

    public const string EmailRequired = "Email is required.";

    public const string MessageRequired = "Message is required.";
    public static string MessageTooShort(int minLength) => $"Message must be at least {minLength} characters.";
    public static string MessageTooLong(int maxLength) => $"Message must be at most {maxLength} characters.";
}
