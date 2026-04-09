namespace CoreFitness.Presentation.WebApp.Models.MyAccount;

public sealed class MyMembershipsViewModel
{
    public bool HasMembership { get; init; }
    public CurrentMembershipViewModel? CurrentMembership { get; init; }
    public IReadOnlyList<MembershipPlanViewModel> AvailablePlans { get; init; } = [];
}

public sealed class CurrentMembershipViewModel
{
    public Guid MembershipId { get; init; }
    public Guid MembershipTypeId { get; init; }
    public string MembershipName { get; init; } = string.Empty;
    public decimal PricePerMonth { get; init; }
    public DateOnly StartDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public IReadOnlyList<string> Benefits { get; init; } = [];
}

public sealed class MembershipPlanViewModel
{
    public Guid MembershipTypeId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal PricePerMonth { get; init; }
    public IReadOnlyList<string> Benefits { get; init; } = [];
}
