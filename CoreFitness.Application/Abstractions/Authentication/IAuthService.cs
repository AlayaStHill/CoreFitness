using CoreFitness.Application.Abstractions.Authentication.Inputs;
using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.Abstractions.Authentication;

public interface IAuthService
{
    Task<Result> SignUpUserAsync(SignUpUserInput input);
    Task<Result> SignInExternalUserAsync(string? roleName);
}


