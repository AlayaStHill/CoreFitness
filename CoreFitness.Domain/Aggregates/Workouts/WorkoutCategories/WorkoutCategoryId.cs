namespace CoreFitness.Domain.Aggregates.Workouts.WorkoutCategories;

public sealed record WorkoutCategoryId(Guid Value)
{
    public static WorkoutCategoryId Create()
    {
        return new WorkoutCategoryId(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
