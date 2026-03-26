namespace CoreFitness.Infrastructure.Identity.Services;

public class IdentityAuthErrors
{
    public const string UserAlreadyExists = "User with the same email already exists.";

    public const string EmailIsRequired = "Email address must be provided";

    public const string PasswordIsRequired = "Password must be provided";
}
