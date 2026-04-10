namespace CoreFitness.Application.MyAccount.Outputs;

public sealed record MyAccountUserOutput(
    string Id,
    string Email,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? ImageUrl
);
