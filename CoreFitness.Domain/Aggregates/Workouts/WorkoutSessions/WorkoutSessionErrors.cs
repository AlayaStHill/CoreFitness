namespace CoreFitness.Domain.Aggregates.Workouts.WorkoutSessions;

public class WorkoutSessionErrors
{
    public const string InvalidStartsAt = "StartsAt must be in the future.";
    public const string InvalidDuration = "Duration must be greater than zero.";
    public const string WorkoutTypeIdRequired = "WorkoutTypeId is required.";
}
