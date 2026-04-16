using CoreFitness.Infrastructure.IntegrationTests.Persistence.Common;
using CoreFitness.Infrastructure.Persistence.EfCore.Repositories.MembershipTypes;

namespace CoreFitness.Infrastructure.IntegrationTests.Persistence.Repositories.MembershipTypes;

[Collection(SqliteInMemoryCollection.Name)]
public sealed class MembershipTypeRepositoryTests(SqliteInMemoryFixture fixture) : PersistenceIntegrationTestBase(fixture)
{
    [Fact]
    public async Task GetFeaturedAsync_ShouldReturnOnlyFeaturedMembershipTypes()
    {
        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.MembershipTypes.AddRange(
                PersistenceTestData.CreateMembershipType("Gold", 599m, true, benefits: ["Spa", "Pool"]),
                PersistenceTestData.CreateMembershipType("Bronze", 199m, false, benefits: ["Gym"]));

            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new MembershipTypeRepository(assertContext);

        var result = await sut.GetFeaturedAsync(CancellationToken.None);

        var featured = Assert.Single(result);
        Assert.Equal("Gold", featured.Name);
        Assert.Equal(599m, featured.PricePerMonth);
        Assert.Equal(["Pool", "Spa"], featured.Benefits.OrderBy(benefit => benefit));
    }

    [Fact]
    public async Task GetFeaturedAsync_ShouldReturnEmptyList_WhenNoFeaturedMembershipTypesExist()
    {
        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.MembershipTypes.Add(
                PersistenceTestData.CreateMembershipType("Bronze", 199m, false, benefits: ["Gym"]));

            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new MembershipTypeRepository(assertContext);

        var result = await sut.GetFeaturedAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetFeaturedAsync_ShouldReturnFeaturedMembershipTypesOrderedByPriceAscending()
    {
        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.MembershipTypes.AddRange(
                PersistenceTestData.CreateMembershipType("Gold", 599m, true, benefits: ["Gym"]),
                PersistenceTestData.CreateMembershipType("Silver", 399m, true, benefits: ["Spa"]),
                PersistenceTestData.CreateMembershipType("Bronze", 199m, false, benefits: ["Pool"]));

            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new MembershipTypeRepository(assertContext);

        var result = await sut.GetFeaturedAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Collection(result,
            first => Assert.Equal("Silver", first.Name),
            second => Assert.Equal("Gold", second.Name));
    }
}
