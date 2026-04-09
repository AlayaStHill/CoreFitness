using CoreFitness.Application.Members;
using CoreFitness.Application.MembershipTypes;
using CoreFitness.Application.MyAccount.Outputs;

namespace CoreFitness.Application.MyAccount;

public sealed class MyAccountMembershipService(
    IMemberRepository memberRepository,
    IMembershipTypeRepository membershipTypeRepository) : IMyAccountMembershipService
{
    public async Task<MyMembershipsOverviewOutput?> GetOverviewAsync(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return null;

        var member = await memberRepository.GetByUserIdWithMembershipsAsync(userId, ct);
        if (member is null)
            return null;

        var featuredPlans = await membershipTypeRepository.GetFeaturedAsync(ct);

        var availablePlans = featuredPlans
            .Select(plan => new MembershipPlanOutput(
                plan.Id,
                plan.Name,
                plan.PricePerMonth,
                plan.Benefits))
            .ToList();

        var activeMembership = member.GetActiveMembership();
        if (activeMembership is null)
        {
            return new MyMembershipsOverviewOutput(
                HasActiveMembership: false,
                ActiveMembership: null,
                AvailablePlans: availablePlans);
        }

        var activePlan = featuredPlans.FirstOrDefault(plan => plan.Id == activeMembership.MembershipTypeId.Value);

        var activeOutput = new ActiveMembershipOutput(
            MembershipId: activeMembership.Id.Value,
            MembershipTypeId: activeMembership.MembershipTypeId.Value,
            MembershipName: activePlan?.Name ?? "Current membership",
            PricePerMonth: activePlan?.PricePerMonth ?? 0m,
            StartDate: activeMembership.StartDate,
            Status: activeMembership.Status.ToString(),
            Benefits: activePlan?.Benefits ?? []);

        return new MyMembershipsOverviewOutput(
            HasActiveMembership: true,
            ActiveMembership: activeOutput,
            AvailablePlans: availablePlans);
    }
}
