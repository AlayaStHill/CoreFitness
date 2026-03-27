namespace CoreFitness.Domain.Aggregates.Members.Memberships;

public sealed record MembershipId(Guid Value)
{
    public static MembershipId Create()
    {
        return new MembershipId(Guid.NewGuid());
    }
    public override string ToString()
    {
        return Value.ToString();
    }
}
