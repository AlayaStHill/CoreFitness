namespace CoreFitness.Application.MyAccount.Outputs;

public sealed record MembershipPlanOutput(
    Guid MembershipTypeId,
    string Name,
    decimal PricePerMonth,
    IReadOnlyList<string> Benefits
);
