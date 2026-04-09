namespace CoreFitness.Application.MyAccount.Outputs;

public sealed record MyMembershipsOverviewOutput(
    bool HasActiveMembership,
    ActiveMembershipOutput? ActiveMembership,
    IReadOnlyList<MembershipPlanOutput> AvailablePlans
);
