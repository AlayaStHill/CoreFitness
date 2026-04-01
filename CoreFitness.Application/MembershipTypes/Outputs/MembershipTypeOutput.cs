namespace CoreFitness.Application.MembershipTypes.Outputs;

public sealed class MembershipTypeFeaturedOutput
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal PricePerMonth { get; init; }
    public int ClassesPerMonth { get; init; }
    public IReadOnlyList<string> Benefits { get; init; } = [];
}
