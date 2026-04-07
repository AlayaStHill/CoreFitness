namespace CoreFitness.Application.Abstractions.Authentication.Inputs;

public sealed record SignInUserInput(string Email, string Password, bool RememberMe = false);
