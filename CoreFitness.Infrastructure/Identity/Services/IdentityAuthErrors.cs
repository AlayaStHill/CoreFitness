namespace CoreFitness.Infrastructure.Identity.Services;

public class IdentityAuthErrors
{
    public const string UserAlreadyExists = "User with the same email already exists.";

    public const string EmailIsRequired = "Email address must be provided";

    public const string PasswordIsRequired = "Password must be provided";

    public const string ExternalLoginInfoMissing = "Could not load external login information"; 

    public const string InvalidCredentials = "Incorrect email address or password";

    public const string UserLockedOut = "This user is temporarily locked out";  

    public const string UserNotAllowed = "This user is not allowed to sign in";

    public const string TwoFactorRequired = "This user requires two-factor authentication";
}
