using CoreFitness.Domain.Aggregates.WorkoutCategories;
using CoreFitness.Domain.Tests.Common;

namespace CoreFitness.Domain.Tests.Aggregates.WorkoutCategories;

public sealed class WorkoutCategoryTests
{
    [Fact]
    public void Create_ShouldTrimTitle_WhenInputHasWhitespace()
    {
        var category = WorkoutCategory.Create("  Strength  ");

        Assert.Equal("Strength", category.Title);
    }

    [Fact]
    public void Create_ShouldThrow_WhenTitleIsMissing()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => WorkoutCategory.Create("  "),
            WorkoutCategoryErrors.TitleRequired);
    }

    [Fact]
    public void Rename_ShouldUpdateTitle_WhenInputIsValid()
    {
        var category = WorkoutCategory.Create("Strength");

        category.Rename("Cardio");

        Assert.Equal("Cardio", category.Title);
    }

    [Fact]
    public void Rename_ShouldThrow_WhenTitleIsMissing()
    {
        var category = WorkoutCategory.Create("Strength");

        ValidationExceptionAssert.ThrowsWithMessage(
            () => category.Rename(""),
            WorkoutCategoryErrors.TitleRequired);
    }
}
