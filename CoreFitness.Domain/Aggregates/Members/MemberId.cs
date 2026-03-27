namespace CoreFitness.Domain.Aggregates.Members;

public sealed record MemberId(Guid Value)
{
    public static MemberId Create()
    {
        return new MemberId(Guid.NewGuid());
    }
    public override string ToString()
    {
        return Value.ToString();
    }

}
