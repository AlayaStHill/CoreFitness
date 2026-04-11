using CoreFitness.Application.Abstractions.Authentication.Inputs;
using CoreFitness.Application.Abstractions.Authentication.Outputs;
using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.Abstractions.Authentication;

public interface IAuthService
{
    Task<Result> SignUpUserAsync(SignUpUserInput input, CancellationToken ct = default);
    Task<Result> SignInExternalUserAsync(string? roleName);
    Task<Result<SignInOutput>> SignInUserAsync(SignInUserInput request);
    Task SignOutUserAsync();

}

