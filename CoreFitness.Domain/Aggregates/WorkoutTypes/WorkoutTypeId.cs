namespace CoreFitness.Domain.Aggregates.WorkoutTypes;

public sealed record WorkoutTypeId(Guid Value)
{
    public static WorkoutTypeId Create()
    {
        return new WorkoutTypeId(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
