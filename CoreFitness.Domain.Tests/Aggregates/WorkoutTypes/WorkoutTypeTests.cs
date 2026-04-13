using CoreFitness.Domain.Aggregates.WorkoutCategories;
using CoreFitness.Domain.Aggregates.WorkoutTypes;
using CoreFitness.Domain.Tests.Common;

namespace CoreFitness.Domain.Tests.Aggregates.WorkoutTypes;

public sealed class WorkoutTypeTests
{
    [Fact]
    public void Create_ShouldThrow_WhenTitleIsMissing()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => WorkoutType.Create(" ", WorkoutCategoryId.Create()),
            WorkoutTypeErrors.TitleRequired);
    }

    [Fact]
    public void Create_ShouldThrow_WhenCategoryIdIsEmpty()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => WorkoutType.Create("Yoga", new WorkoutCategoryId(Guid.Empty)),
            WorkoutTypeErrors.WorkoutCategoryIdRequired);
    }

    [Fact]
    public void Rename_ShouldUpdateTitle_WhenInputIsValid()
    {
        var workoutType = WorkoutType.Create("Yoga", WorkoutCategoryId.Create());

        workoutType.Rename("Pilates");

        Assert.Equal("Pilates", workoutType.Title);
    }

    [Fact]
    public void ChangeCategory_ShouldUpdateCategory_WhenInputIsValid()
    {
        var workoutType = WorkoutType.Create("Yoga", WorkoutCategoryId.Create());
        var newCategoryId = WorkoutCategoryId.Create();

        workoutType.ChangeCategory(newCategoryId);

        Assert.Equal(newCategoryId, workoutType.WorkoutCategoryId);
    }

    [Fact]
    public void ChangeCategory_ShouldThrow_WhenCategoryIdIsEmpty()
    {
        var workoutType = WorkoutType.Create("Yoga", WorkoutCategoryId.Create());

        ValidationExceptionAssert.ThrowsWithMessage(
            () => workoutType.ChangeCategory(new WorkoutCategoryId(Guid.Empty)),
            WorkoutTypeErrors.WorkoutCategoryIdRequired);
    }
}
