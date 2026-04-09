namespace CoreFitness.Application.MyAccount.Outputs;

public sealed record CurrentMembershipOutput(
    Guid MembershipId,
    Guid MembershipTypeId,
    string MembershipName,
    decimal PricePerMonth,
    DateOnly StartDate,
    string Status,
    IReadOnlyList<string> Benefits
);
