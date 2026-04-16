using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;

namespace CoreFitness.Infrastructure.IntegrationTests.Persistence.Common;

public abstract class PersistenceIntegrationTestBase(SqliteInMemoryFixture fixture) : IAsyncLifetime
{
    protected SqliteInMemoryFixture Fixture { get; } = fixture;

    protected PersistenceContext CreateContext() => Fixture.CreateContext();

    public Task InitializeAsync() => Fixture.ClearDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;
}
