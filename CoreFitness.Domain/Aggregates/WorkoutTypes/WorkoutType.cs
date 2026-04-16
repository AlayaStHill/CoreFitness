using corefitness.domain.shared.validators;
using CoreFitness.Domain.Aggregates.WorkoutCategories;

namespace CoreFitness.Domain.Aggregates.WorkoutTypes;

public sealed class WorkoutType
{
    public WorkoutTypeId Id { get; private set; } = default!;
    public string Title { get; private set; } = null!;
    public WorkoutCategoryId WorkoutCategoryId { get; private set; } = default!;
    // navigationproperty EF
    public WorkoutCategory WorkoutCategory { get; private set; } = default!;

    private WorkoutType(WorkoutTypeId id, string title, WorkoutCategoryId workoutCategoryId)
    {
        Id = id;
        Title = title;
        WorkoutCategoryId = workoutCategoryId;
    }
    private WorkoutType() { }

    public static WorkoutType Create(string title, WorkoutCategoryId workoutCategoryId)
    {
        var normalizedTitle = DomainValidator.RequiredString(title, WorkoutTypeErrors.TitleRequired);
        DomainValidator.RequiredGuid(workoutCategoryId.Value, WorkoutTypeErrors.WorkoutCategoryIdRequired);

        return new(WorkoutTypeId.Create(), normalizedTitle, workoutCategoryId);
    }

    public void Rename(string title)
    {
        Title = DomainValidator.RequiredString(title, WorkoutTypeErrors.TitleRequired);
    }

    public void ChangeCategory(WorkoutCategoryId workoutCategoryId)
    {
        DomainValidator.RequiredGuid(workoutCategoryId.Value, WorkoutTypeErrors.WorkoutCategoryIdRequired);
        WorkoutCategoryId = workoutCategoryId;
    }
}
