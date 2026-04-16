using CoreFitness.Infrastructure.IntegrationTests.Persistence.Common;
using CoreFitness.Infrastructure.Persistence.EfCore.Repositories.Members;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.IntegrationTests.Persistence.Repositories.Members;

[Collection(SqliteInMemoryCollection.Name)]
public sealed class MemberRepositoryTests(SqliteInMemoryFixture fixture) : PersistenceIntegrationTestBase(fixture)
{
    [Fact]
    public async Task AddAsync_ShouldPersistMember()
    {
        const string userId = "user-1";
        var user = PersistenceTestData.CreateApplicationUser(userId, "user1@corefitness.test");
        var member = PersistenceTestData.CreateMember(userId);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.Users.Add(user);

            var sut = new MemberRepository(arrangeContext);
            await sut.AddAsync(member, CancellationToken.None);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var persisted = await assertContext.Members.FirstOrDefaultAsync(x => x.UserId == userId, CancellationToken.None);

        Assert.NotNull(persisted);
        Assert.Equal(userId, persisted.UserId);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPersistedMembers()
    {
        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.Users.AddRange(
                PersistenceTestData.CreateApplicationUser("user-1", "user1@corefitness.test"),
                PersistenceTestData.CreateApplicationUser("user-2", "user2@corefitness.test"));

            arrangeContext.Members.AddRange(
                PersistenceTestData.CreateMember("user-1"),
                PersistenceTestData.CreateMember("user-2"));

            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new MemberRepository(assertContext);

        var result = await sut.GetAllAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByUserIdWithMembershipsAsync_ShouldIncludeMemberships_WhenMembershipsExist()
    {
        const string userId = "member-with-membership";
        var membershipType = PersistenceTestData.CreateMembershipType("Gold", 499m, true, benefits: ["Gym"]);
        var user = PersistenceTestData.CreateApplicationUser(userId, "member.with.membership@corefitness.test");
        var member = PersistenceTestData.CreateMemberWithActiveMembership(membershipType.Id, userId);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.Users.Add(user);
            arrangeContext.MembershipTypes.Add(membershipType);
            arrangeContext.Members.Add(member);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new MemberRepository(assertContext);

        var result = await sut.GetByUserIdWithMembershipsAsync(userId, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Memberships);
    }

    [Fact]
    public async Task RemoveByUserIdAsync_ShouldReturnFalse_WhenMemberDoesNotExist()
    {
        await using var context = CreateContext();
        var sut = new MemberRepository(context);

        var result = await sut.RemoveByUserIdAsync("missing-user", CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task RemoveByUserIdAsync_ShouldDeleteMemberMembershipsAndBookings_WhenMemberExists()
    {
        const string userId = "member-to-remove";
        var membershipType = PersistenceTestData.CreateMembershipType("Silver", 399m, true, benefits: ["Gym"]);
        var user = PersistenceTestData.CreateApplicationUser(userId, "member.to.remove@corefitness.test");
        var member = PersistenceTestData.CreateMemberWithActiveMembership(membershipType.Id, userId);
        var category = PersistenceTestData.CreateWorkoutCategory("Functional");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Circuit");
        var workoutSession = PersistenceTestData.CreateWorkoutSession(workoutType.Id);

        PersistenceTestData.BookWorkoutSession(workoutSession, member.Id);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.Users.Add(user);
            arrangeContext.MembershipTypes.Add(membershipType);
            arrangeContext.Members.Add(member);
            arrangeContext.WorkoutCategories.Add(category);
            arrangeContext.WorkoutTypes.Add(workoutType);
            arrangeContext.WorkoutSessions.Add(workoutSession);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using (var actContext = CreateContext())
        {
            var sut = new MemberRepository(actContext);
            var removed = await sut.RemoveByUserIdAsync(userId, CancellationToken.None);
            await actContext.SaveChangesAsync(CancellationToken.None);

            Assert.True(removed);
        }

        await using var assertContext = CreateContext();
        var deletedMember = await assertContext.Members.FirstOrDefaultAsync(x => x.UserId == userId, CancellationToken.None);
        var membershipsCount = await assertContext.Memberships.CountAsync(CancellationToken.None);
        var bookingsCount = await assertContext.Bookings.CountAsync(CancellationToken.None);

        Assert.Null(deletedMember);
        Assert.Equal(0, membershipsCount);
        Assert.Equal(0, bookingsCount);
    }
}
