using CoreFitness.Infrastructure.IntegrationTests.Persistence.Common;
using CoreFitness.Infrastructure.Persistence.EfCore.Repositories.CustomerService;

namespace CoreFitness.Infrastructure.IntegrationTests.Persistence.Repositories.CustomerService;

[Collection(SqliteInMemoryCollection.Name)]
public sealed class ContactRequestRepositoryTests(SqliteInMemoryFixture fixture) : PersistenceIntegrationTestBase(fixture)
{
    [Fact]
    public async Task AddAsync_ShouldPersistContactRequest()
    {
        var contactRequest = PersistenceTestData.CreateContactRequest();

        await using (var arrangeContext = CreateContext())
        {
            var sut = new ContactRequestRepository(arrangeContext);
            await sut.AddAsync(contactRequest, CancellationToken.None);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var persisted = await assertContext.ContactRequests.FindAsync([contactRequest.Id], CancellationToken.None);

        Assert.NotNull(persisted);
        Assert.Equal("John", persisted.FirstName);
        Assert.Equal("Doe", persisted.LastName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnContactRequest_WhenEntityExists()
    {
        var contactRequest = PersistenceTestData.CreateContactRequest(firstName: "Alice");

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.ContactRequests.Add(contactRequest);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new ContactRequestRepository(assertContext);

        var result = await sut.GetByIdAsync(contactRequest.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Alice", result.FirstName);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPersistedContactRequests()
    {
        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.ContactRequests.AddRange(
                PersistenceTestData.CreateContactRequest(firstName: "Anna", email: "anna@doe.com"),
                PersistenceTestData.CreateContactRequest(firstName: "Bob", email: "bob@doe.com"));

            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new ContactRequestRepository(assertContext);

        var result = await sut.GetAllAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoEntitiesExist()
    {
        await using var context = CreateContext();
        var sut = new ContactRequestRepository(context);

        var result = await sut.GetAllAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnTrueAndDeleteEntity_WhenEntityExists()
    {
        var contactRequest = PersistenceTestData.CreateContactRequest();

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.ContactRequests.Add(contactRequest);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using (var actContext = CreateContext())
        {
            var sut = new ContactRequestRepository(actContext);
            var removed = await sut.RemoveAsync(contactRequest.Id, CancellationToken.None);
            await actContext.SaveChangesAsync(CancellationToken.None);

            Assert.True(removed);
        }

        await using var assertContext = CreateContext();
        var deleted = await assertContext.ContactRequests.FindAsync([contactRequest.Id], CancellationToken.None);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        await using var context = CreateContext();
        var sut = new ContactRequestRepository(context);
        var updatedModel = PersistenceTestData.CreateContactRequest(firstName: "Updated");

        var result = await sut.UpdateAsync("missing-id", updatedModel, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        await using var context = CreateContext();
        var sut = new ContactRequestRepository(context);

        var result = await sut.GetByIdAsync("missing-id", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnFalse_WhenEntityDoesNotExist()
    {
        await using var context = CreateContext();
        var sut = new ContactRequestRepository(context);

        var result = await sut.RemoveAsync("missing-id", CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldPersistChanges_WhenEntityExists()
    {
        var contactRequest = PersistenceTestData.CreateContactRequest();

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.ContactRequests.Add(contactRequest);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using (var actContext = CreateContext())
        {
            var entityToUpdate = await actContext.ContactRequests.FindAsync([contactRequest.Id], CancellationToken.None);
            Assert.NotNull(entityToUpdate);

            entityToUpdate.MarkAsRead();

            var sut = new ContactRequestRepository(actContext);
            var updated = await sut.UpdateAsync(contactRequest.Id, entityToUpdate, CancellationToken.None);
            await actContext.SaveChangesAsync(CancellationToken.None);

            Assert.NotNull(updated);
            Assert.True(updated.MarkedAsRead);
        }

        await using var assertContext = CreateContext();
        var persisted = await assertContext.ContactRequests.FindAsync([contactRequest.Id], CancellationToken.None);
        Assert.NotNull(persisted);
        Assert.True(persisted.MarkedAsRead);
    }

    [Fact]
    public async Task AddAsync_ShouldThrowArgumentNullException_WhenModelIsNull()
    {
        await using var context = CreateContext();
        var sut = new ContactRequestRepository(context);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => sut.AddAsync(null!, CancellationToken.None));

        Assert.Equal("model", exception.ParamName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowArgumentNullException_WhenModelIsNull()
    {
        await using var context = CreateContext();
        var sut = new ContactRequestRepository(context);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => sut.UpdateAsync("id-1", null!, CancellationToken.None));

        Assert.Equal("model", exception.ParamName);
    }
}
