using CoreFitness.Application.Members;
using CoreFitness.Application.MembershipTypes;
using CoreFitness.Application.MyAccount.Outputs;
using CoreFitness.Application.Shared;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Application.MyAccount;

public sealed class MyAccountMembershipService(
    IMemberRepository memberRepository,
    IMembershipTypeRepository membershipTypeRepository,
    IUnitOfWork unitOfWork) : IMyAccountMembershipService
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

    public async Task<Result> SelectPlanAsync(string userId, Guid membershipTypeId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Fail(ErrorTypes.BadRequest, "User id must be provided.");

        if (membershipTypeId == Guid.Empty)
            return Result.Fail(ErrorTypes.BadRequest, "Membership type id must be provided.");

        var member = await memberRepository.GetByUserIdWithMembershipsAsync(userId, ct);
        if (member is null)
            return Result.Fail(ErrorTypes.NotFound, "Member not found.");

        var availablePlans = await membershipTypeRepository.GetFeaturedAsync(ct);
        if (!availablePlans.Any(plan => plan.Id == membershipTypeId))
            return Result.Fail(ErrorTypes.NotFound, "Selected membership plan was not found.");

        try
        {
            member.StartMembership(new MembershipTypeId(membershipTypeId));
        }
        catch (ValidationDomainException ex)
        {
            return Result.Fail(ErrorTypes.BadRequest, ex.Message);
        }

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
