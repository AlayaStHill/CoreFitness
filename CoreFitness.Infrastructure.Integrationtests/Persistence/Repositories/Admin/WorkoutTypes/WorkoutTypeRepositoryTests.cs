using CoreFitness.Domain.Aggregates.WorkoutCategories;
using CoreFitness.Domain.Aggregates.WorkoutTypes;
using CoreFitness.Infrastructure.IntegrationTests.Persistence.Common;
using CoreFitness.Infrastructure.Persistence.EfCore.Repositories.Admin.WorkoutTypes;

namespace CoreFitness.Infrastructure.IntegrationTests.Persistence.Repositories.Admin.WorkoutTypes;

[Collection(SqliteInMemoryCollection.Name)]
public sealed class WorkoutTypeRepositoryTests(SqliteInMemoryFixture fixture) : PersistenceIntegrationTestBase(fixture)
{
    [Fact]
    public async Task AddAsync_ShouldPersistWorkoutType()
    {
        var category = PersistenceTestData.CreateWorkoutCategory("Strength");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Crossfit");

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.WorkoutCategories.Add(category);

            var sut = new WorkoutTypeRepository(arrangeContext);
            await sut.AddAsync(workoutType, CancellationToken.None);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var persisted = await assertContext.WorkoutTypes.FindAsync([workoutType.Id], CancellationToken.None);

        Assert.NotNull(persisted);
        Assert.Equal("Crossfit", persisted.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnWorkoutType_WhenEntityExists()
    {
        var category = PersistenceTestData.CreateWorkoutCategory("Cardio");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Spinning");

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.WorkoutCategories.Add(category);
            arrangeContext.WorkoutTypes.Add(workoutType);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new WorkoutTypeRepository(assertContext);

        var result = await sut.GetByIdAsync(workoutType.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Spinning", result.Title);
    }

    [Fact]
    public async Task GetAllAsync_ShouldIncludeWorkoutCategory()
    {
        var category = PersistenceTestData.CreateWorkoutCategory("Functional");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Circuit");

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.WorkoutCategories.Add(category);
            arrangeContext.WorkoutTypes.Add(workoutType);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new WorkoutTypeRepository(assertContext);

        var result = await sut.GetAllAsync(CancellationToken.None);

        var item = Assert.Single(result);
        Assert.Equal("Circuit", item.Title);
        Assert.NotNull(item.WorkoutCategory);
        Assert.Equal("Functional", item.WorkoutCategory.Title);
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnTrueAndDeleteEntity_WhenEntityExists()
    {
        var category = PersistenceTestData.CreateWorkoutCategory("Mobility");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Stretch");

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.WorkoutCategories.Add(category);
            arrangeContext.WorkoutTypes.Add(workoutType);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using (var actContext = CreateContext())
        {
            var sut = new WorkoutTypeRepository(actContext);
            var removed = await sut.RemoveAsync(workoutType.Id, CancellationToken.None);
            await actContext.SaveChangesAsync(CancellationToken.None);

            Assert.True(removed);
        }

        await using var assertContext = CreateContext();
        var deleted = await assertContext.WorkoutTypes.FindAsync([workoutType.Id], CancellationToken.None);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        await using var context = CreateContext();
        var sut = new WorkoutTypeRepository(context);
        var categoryId = WorkoutCategoryId.Create();
        var updatedModel = PersistenceTestData.CreateWorkoutType(categoryId, "Updated");

        var result = await sut.UpdateAsync(WorkoutTypeId.Create(), updatedModel, CancellationToken.None);

        Assert.Null(result);
    }
}
