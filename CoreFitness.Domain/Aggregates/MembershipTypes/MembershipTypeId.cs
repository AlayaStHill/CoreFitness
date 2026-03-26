namespace CoreFitness.Domain.Aggregates.MembershipTypes;

public sealed record MembershipTypeId(Guid Value)
{
    public static MembershipTypeId Create()
    {
        return new MembershipTypeId(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
