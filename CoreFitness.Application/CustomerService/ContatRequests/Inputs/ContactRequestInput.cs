namespace CoreFitness.Application.CustomerService.ContatRequests.Inputs;

public sealed record ContactRequestInput(string FirstName, string LastName, string Email, string? PhoneNumber, string Message);

