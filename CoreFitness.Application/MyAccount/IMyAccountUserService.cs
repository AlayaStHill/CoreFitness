using CoreFitness.Application.MyAccount.Inputs;
using CoreFitness.Application.MyAccount.Outputs;
using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.MyAccount;

public interface IMyAccountUserService
{
    Task<MyAccountUserOutput?> GetByIdAsync(string userId);
    Task<Result> UpdateAsync(UpdateMyAccountUserInput input);
}
