namespace CoreFitness.Domain.Aggregates.Workouts.WorkoutSessions;

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
