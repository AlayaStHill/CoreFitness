namespace CoreFitness.Domain.Aggregates.CustomerService;

public static class ContactRequestErrors
{
    public const string FirstNameRequired = "First name is required.";
    public const string FirstNameTooShort = "First name must be at least 2 characters.";
    public const string FirstNameTooLong = "First name is too long.";

    public const string LastNameRequired = "Last name is required.";
    public const string LastNameTooShort = "Last name must be at least 2 characters.";
    public const string LastNameTooLong = "Last name is too long.";

    public const string EmailRequired = "Email is required.";

    public const string MessageRequired = "Message is required.";
    public const string MessageTooShort = "Message must be at least 5 characters.";
}
