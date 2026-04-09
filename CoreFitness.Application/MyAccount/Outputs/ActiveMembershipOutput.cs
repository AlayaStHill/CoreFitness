namespace CoreFitness.Application.MyAccount.Outputs;

public sealed record ActiveMembershipOutput(
    Guid MembershipId,
    Guid MembershipTypeId,
    string MembershipName,
    decimal PricePerMonth,
    DateOnly StartDate,
    string Status,
    IReadOnlyList<string> Benefits
);
