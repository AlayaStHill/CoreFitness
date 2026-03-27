namespace CoreFitness.Domain.Aggregates.MembershipTypes.MembershipBenefits;

public sealed record MembershipBenefitId(Guid Value)
{
    public static MembershipBenefitId Create() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();

}
