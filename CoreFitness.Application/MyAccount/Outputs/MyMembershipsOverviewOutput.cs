namespace CoreFitness.Application.MyAccount.Outputs;

public sealed record MyMembershipsOverviewOutput(
    bool HasMembership,
    CurrentMembershipOutput? CurrentMembership,
    IReadOnlyList<MembershipPlanOutput> AvailablePlans
);
