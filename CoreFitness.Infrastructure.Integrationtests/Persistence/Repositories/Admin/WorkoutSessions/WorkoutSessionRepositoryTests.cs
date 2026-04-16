using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Domain.Aggregates.WorkoutTypes;
using CoreFitness.Infrastructure.IntegrationTests.Persistence.Common;
using CoreFitness.Infrastructure.Persistence.EfCore.Repositories.Admin.WorkoutSessions;

namespace CoreFitness.Infrastructure.IntegrationTests.Persistence.Repositories.Admin.WorkoutSessions;

[Collection(SqliteInMemoryCollection.Name)]
public sealed class WorkoutSessionRepositoryTests(SqliteInMemoryFixture fixture) : PersistenceIntegrationTestBase(fixture)
{
    [Fact]
    public async Task AddAsync_ShouldPersistWorkoutSession()
    {
        var category = PersistenceTestData.CreateWorkoutCategory("Strength");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Crossfit");
        var workoutSession = PersistenceTestData.CreateWorkoutSession(workoutType.Id);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.WorkoutCategories.Add(category);
            arrangeContext.WorkoutTypes.Add(workoutType);

            var sut = new WorkoutSessionRepository(arrangeContext);
            await sut.AddAsync(workoutSession, CancellationToken.None);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var persisted = await assertContext.WorkoutSessions.FindAsync([workoutSession.Id], CancellationToken.None);

        Assert.NotNull(persisted);
        Assert.Equal(workoutType.Id, persisted.WorkoutTypeId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnWorkoutSession_WhenEntityExists()
    {
        var category = PersistenceTestData.CreateWorkoutCategory("Cardio");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Spinning");
        var workoutSession = PersistenceTestData.CreateWorkoutSession(workoutType.Id);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.WorkoutCategories.Add(category);
            arrangeContext.WorkoutTypes.Add(workoutType);
            arrangeContext.WorkoutSessions.Add(workoutSession);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new WorkoutSessionRepository(assertContext);

        var result = await sut.GetByIdAsync(workoutSession.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(workoutSession.StartsAt, result.StartsAt);
    }

    [Fact]
    public async Task GetAllAsync_ShouldIncludeWorkoutTypeAndWorkoutCategory()
    {
        var category = PersistenceTestData.CreateWorkoutCategory("Functional");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Circuit");
        var workoutSession = PersistenceTestData.CreateWorkoutSession(workoutType.Id);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.WorkoutCategories.Add(category);
            arrangeContext.WorkoutTypes.Add(workoutType);
            arrangeContext.WorkoutSessions.Add(workoutSession);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new WorkoutSessionRepository(assertContext);

        var result = await sut.GetAllAsync(CancellationToken.None);

        var item = Assert.Single(result);
        Assert.NotNull(item.WorkoutType);
        Assert.Equal("Circuit", item.WorkoutType.Title);
        Assert.NotNull(item.WorkoutType.WorkoutCategory);
        Assert.Equal("Functional", item.WorkoutType.WorkoutCategory.Title);
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnTrueAndDeleteEntity_WhenEntityExists()
    {
        var category = PersistenceTestData.CreateWorkoutCategory("Mobility");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Stretch");
        var workoutSession = PersistenceTestData.CreateWorkoutSession(workoutType.Id);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.WorkoutCategories.Add(category);
            arrangeContext.WorkoutTypes.Add(workoutType);
            arrangeContext.WorkoutSessions.Add(workoutSession);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using (var actContext = CreateContext())
        {
            var sut = new WorkoutSessionRepository(actContext);
            var removed = await sut.RemoveAsync(workoutSession.Id, CancellationToken.None);
            await actContext.SaveChangesAsync(CancellationToken.None);

            Assert.True(removed);
        }

        await using var assertContext = CreateContext();
        var deleted = await assertContext.WorkoutSessions.FindAsync([workoutSession.Id], CancellationToken.None);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        await using var context = CreateContext();
        var sut = new WorkoutSessionRepository(context);
        var workoutTypeId = WorkoutTypeId.Create();
        var updatedModel = PersistenceTestData.CreateWorkoutSession(workoutTypeId);

        var result = await sut.UpdateAsync(WorkoutSessionId.Create(), updatedModel, CancellationToken.None);

        Assert.Null(result);
    }
}
