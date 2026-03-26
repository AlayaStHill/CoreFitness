using corefitness.domain.shared.validators;

namespace CoreFitness.Domain.Aggregates.WorkoutCategories;

public sealed class WorkoutCategory
{
    public WorkoutCategoryId Id { get; private set; } = default!;
    public string Title { get; private set; } = null!;
    private WorkoutCategory(WorkoutCategoryId id, string title)
    {
        Id = id;
        Title = title;
    }
    private WorkoutCategory() { }
    public static WorkoutCategory Create(string title)
    {
        var normalizedTitle = DomainValidator.RequiredString(title, WorkoutCategoryErrors.TitleRequired);
        return new(WorkoutCategoryId.Create(), normalizedTitle);
    }

    public void Rename(string title)
    {
        Title = DomainValidator.RequiredString(title, WorkoutCategoryErrors.TitleRequired);
    }
}
