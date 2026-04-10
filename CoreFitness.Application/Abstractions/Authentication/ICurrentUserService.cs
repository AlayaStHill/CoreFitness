namespace CoreFitness.Application.Abstractions.Authentication;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
}
