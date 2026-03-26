using corefitness.domain.shared.validators;

namespace CoreFitness.Domain.Aggregates.Workouts.WorkoutTypes;

public sealed class WorkoutType
{
    public WorkoutTypeId Id { get; private set; } = default!;
    public string Title { get; private set; } = null!;
    public WorkoutCategoryId WorkoutCategoryId { get; private set; }

    private WorkoutType(WorkoutTypeId id, string title, WorkoutCategoryId workoutCategoryId)
    {
        Id = id;
        Title = title;
        WorkoutCategoryId = workoutCategoryId;
    }

    public static WorkoutType Create(string title, WorkoutCategoryId workoutCategoryId)
    {
        var normalizedTitle = DomainValidator.RequiredString(title, WorkoutTypeErrors.TitleRequired);
        DomainValidator.RequiredGuid(workoutCategoryId.Value, WorkoutTypeErrors.WorkoutCategoryIdRequired);

        return new(WorkoutTypeId.Create(), normalizedTitle, workoutCategoryId);
    }
    public static WorkoutType Rehydrate(string id, string title, string workoutCategoryId) => new(new WorkoutTypeId(new Guid(id)), title, workoutCategoryId);
}
