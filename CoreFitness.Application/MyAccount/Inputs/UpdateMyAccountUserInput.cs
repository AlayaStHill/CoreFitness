namespace CoreFitness.Application.MyAccount.Inputs;

public sealed record UpdateMyAccountUserInput(
    string UserId,
    string Email,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? ImageUrl
);
