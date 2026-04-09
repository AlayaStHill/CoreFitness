using CoreFitness.Application.MyAccount.Outputs;
using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.MyAccount;

public interface IMyAccountMembershipService
{
    Task<MyMembershipsOverviewOutput?> GetOverviewAsync(string userId, CancellationToken ct = default);
    Task<Result> SelectPlanAsync(string userId, Guid membershipTypeId, CancellationToken ct = default);
    Task<Result> CancelActiveMembershipAsync(string userId, CancellationToken ct = default);

}
