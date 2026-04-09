using CoreFitness.Application.MyAccount.Outputs;

namespace CoreFitness.Application.MyAccount;

public interface IMyAccountMembershipService
{
    Task<MyMembershipsOverviewOutput?> GetOverviewAsync(string userId, CancellationToken ct = default);
}
