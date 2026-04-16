namespace CoreFitness.Domain.Aggregates.WorkoutSessions;

public sealed record WorkoutSessionId(Guid Value)
{
    public static WorkoutSessionId Create()
    {
        return new WorkoutSessionId(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
