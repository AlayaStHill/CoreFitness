namespace CoreFitness.Application.Abstractions.Authentication.Outputs;

public sealed class SignInOutput
{
    public bool IsAdmin { get; init; }
    public bool IsMember { get; init; }
}
