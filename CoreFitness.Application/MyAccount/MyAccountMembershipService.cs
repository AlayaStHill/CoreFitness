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

        var currentMembership = member.GetActiveMembership()
            ?? member.Memberships.FirstOrDefault(membership => membership.IsPaused());

        if (currentMembership is null)
        {
            return new MyMembershipsOverviewOutput(
                HasMembership: false,
                CurrentMembership: null,
                AvailablePlans: availablePlans);
        }

        var activePlan = featuredPlans.FirstOrDefault(plan => plan.Id == currentMembership.MembershipTypeId.Value);

        var currentOutput = new CurrentMembershipOutput(
            MembershipId: currentMembership.Id.Value,
            MembershipTypeId: currentMembership.MembershipTypeId.Value,
            MembershipName: activePlan?.Name ?? "Current membership",
            PricePerMonth: activePlan?.PricePerMonth ?? 0m,
            StartDate: currentMembership.StartDate,
            Status: currentMembership.Status.ToString(),
            Benefits: activePlan?.Benefits ?? []);

        return new MyMembershipsOverviewOutput(
            HasMembership: true,
            CurrentMembership: currentOutput,
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

    public async Task<Result> CancelActiveMembershipAsync(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Fail(ErrorTypes.BadRequest, "User id must be provided.");

        var member = await memberRepository.GetByUserIdWithMembershipsAsync(userId, ct);
        if (member is null)
            return Result.Fail(ErrorTypes.NotFound, "Member not found.");

        try
        {
            if (member.HasActiveMembership())
            {
                member.CancelActiveMembership();
            }
            else
            {
                member.ActivatePausedMembership();
                member.CancelActiveMembership();
            }
        }
        catch (ValidationDomainException ex)
        {
            return Result.Fail(ErrorTypes.BadRequest, ex.Message);
        }

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> PauseActiveMembershipAsync(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Fail(ErrorTypes.BadRequest, "User id must be provided.");

        var member = await memberRepository.GetByUserIdWithMembershipsAsync(userId, ct);
        if (member is null)
            return Result.Fail(ErrorTypes.NotFound, "Member not found.");

        try
        {
            member.PauseActiveMembership();
        }
        catch (ValidationDomainException ex)
        {
            return Result.Fail(ErrorTypes.BadRequest, ex.Message);
        }

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> ActivatePausedMembershipAsync(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Fail(ErrorTypes.BadRequest, "User id must be provided.");

        var member = await memberRepository.GetByUserIdWithMembershipsAsync(userId, ct);
        if (member is null)
            return Result.Fail(ErrorTypes.NotFound, "Member not found.");

        try
        {
            member.ActivatePausedMembership();
        }
        catch (ValidationDomainException ex)
        {
            return Result.Fail(ErrorTypes.BadRequest, ex.Message);
        }

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
