using CoreFitness.Infrastructure.IntegrationTests.Persistence.Common;
using CoreFitness.Infrastructure.Persistence.EfCore.Repositories.MyAccount;

namespace CoreFitness.Infrastructure.IntegrationTests.Persistence.Repositories.MyAccount;

[Collection(SqliteInMemoryCollection.Name)]
public sealed class MyAccountBookingRepositoryTests(SqliteInMemoryFixture fixture) : PersistenceIntegrationTestBase(fixture)
{
    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnOnlyBookingsForRequestedUser()
    {
        const string targetUserId = "member-1";
        const string otherUserId = "member-2";

        var targetUser = PersistenceTestData.CreateApplicationUser(targetUserId, "member1@corefitness.test");
        var otherUser = PersistenceTestData.CreateApplicationUser(otherUserId, "member2@corefitness.test");
        var targetMember = PersistenceTestData.CreateMember(targetUserId);
        var otherMember = PersistenceTestData.CreateMember(otherUserId);

        var category = PersistenceTestData.CreateWorkoutCategory("Functional");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Circuit");
        var targetSession = PersistenceTestData.CreateWorkoutSession(workoutType.Id, DateTimeOffset.UtcNow.AddHours(2));
        var otherSession = PersistenceTestData.CreateWorkoutSession(workoutType.Id, DateTimeOffset.UtcNow.AddHours(3));

        PersistenceTestData.BookWorkoutSession(targetSession, targetMember.Id);
        PersistenceTestData.BookWorkoutSession(otherSession, otherMember.Id);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.Users.AddRange(targetUser, otherUser);
            arrangeContext.Members.AddRange(targetMember, otherMember);
            arrangeContext.WorkoutCategories.Add(category);
            arrangeContext.WorkoutTypes.Add(workoutType);
            arrangeContext.WorkoutSessions.AddRange(targetSession, otherSession);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new MyAccountBookingRepository(assertContext);

        var result = await sut.GetByUserIdAsync(targetUserId, CancellationToken.None);

        var booking = Assert.Single(result);
        Assert.Equal(targetSession.Id.Value, booking.WorkoutSessionId);
        Assert.Equal("Circuit", booking.WorkoutTitle);
        Assert.Equal("Functional", booking.WorkoutCategory);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnEmptyList_WhenUserHasNoBookings()
    {
        const string userId = "member-1";
        var user = PersistenceTestData.CreateApplicationUser(userId, "member1@corefitness.test");
        var member = PersistenceTestData.CreateMember(userId);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.Users.Add(user);
            arrangeContext.Members.Add(member);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new MyAccountBookingRepository(assertContext);

        var result = await sut.GetByUserIdAsync(userId, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUpcomingSessionsByUserIdAsync_ShouldReturnEmptyList_WhenMemberDoesNotExist()
    {
        await using var context = CreateContext();
        var sut = new MyAccountBookingRepository(context);

        var result = await sut.GetUpcomingSessionsByUserIdAsync("missing-user", CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUpcomingSessionsByUserIdAsync_ShouldReturnOrderedUpcomingSessionsWithBookingStatus()
    {
        const string targetUserId = "member-1";
        const string otherUserId = "member-2";

        var targetUser = PersistenceTestData.CreateApplicationUser(targetUserId, "member1@corefitness.test");
        var otherUser = PersistenceTestData.CreateApplicationUser(otherUserId, "member2@corefitness.test");
        var targetMember = PersistenceTestData.CreateMember(targetUserId);
        var otherMember = PersistenceTestData.CreateMember(otherUserId);

        var category = PersistenceTestData.CreateWorkoutCategory("Functional");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Circuit");

        var firstSession = PersistenceTestData.CreateWorkoutSession(workoutType.Id, DateTimeOffset.UtcNow.AddHours(1), capacity: 10);
        var secondSession = PersistenceTestData.CreateWorkoutSession(workoutType.Id, DateTimeOffset.UtcNow.AddHours(3), capacity: 12);

        PersistenceTestData.BookWorkoutSession(firstSession, targetMember.Id);
        PersistenceTestData.BookWorkoutSession(firstSession, otherMember.Id);
        PersistenceTestData.BookWorkoutSession(secondSession, otherMember.Id);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.Users.AddRange(targetUser, otherUser);
            arrangeContext.Members.AddRange(targetMember, otherMember);
            arrangeContext.WorkoutCategories.Add(category);
            arrangeContext.WorkoutTypes.Add(workoutType);
            arrangeContext.WorkoutSessions.AddRange(firstSession, secondSession);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new MyAccountBookingRepository(assertContext);

        var result = await sut.GetUpcomingSessionsByUserIdAsync(targetUserId, CancellationToken.None);

        Assert.Equal(2, result.Count);

        Assert.Collection(result,
            first =>
            {
                Assert.Equal(firstSession.Id.Value, first.WorkoutSessionId);
                Assert.Equal(2, first.BookedCount);
                Assert.Equal(10, first.Capacity);
                Assert.True(first.IsAlreadyBooked);
            },
            second =>
            {
                Assert.Equal(secondSession.Id.Value, second.WorkoutSessionId);
                Assert.Equal(1, second.BookedCount);
                Assert.Equal(12, second.Capacity);
                Assert.False(second.IsAlreadyBooked);
            });
    }

    [Fact]
    public async Task GetMemberByUserIdForBookingAsync_ShouldIncludeMemberships_WhenMemberExists()
    {
        const string userId = "member-with-membership";

        var user = PersistenceTestData.CreateApplicationUser(userId, "member.with.membership@corefitness.test");
        var membershipType = PersistenceTestData.CreateMembershipType("Gold", 499m, true, benefits: ["Gym"]);
        var member = PersistenceTestData.CreateMemberWithActiveMembership(membershipType.Id, userId);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.Users.Add(user);
            arrangeContext.MembershipTypes.Add(membershipType);
            arrangeContext.Members.Add(member);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new MyAccountBookingRepository(assertContext);

        var result = await sut.GetMemberByUserIdForBookingAsync(userId, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Memberships);
    }

    [Fact]
    public async Task GetMemberByUserIdForBookingAsync_ShouldReturnNull_WhenMemberDoesNotExist()
    {
        await using var context = CreateContext();
        var sut = new MyAccountBookingRepository(context);

        var result = await sut.GetMemberByUserIdForBookingAsync("missing-user", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetWorkoutSessionByIdForBookingAsync_ShouldIncludeBookings_WhenSessionExists()
    {
        const string userId = "member-1";

        var user = PersistenceTestData.CreateApplicationUser(userId, "member1@corefitness.test");
        var member = PersistenceTestData.CreateMember(userId);
        var category = PersistenceTestData.CreateWorkoutCategory("Functional");
        var workoutType = PersistenceTestData.CreateWorkoutType(category.Id, "Circuit");
        var workoutSession = PersistenceTestData.CreateWorkoutSession(workoutType.Id, DateTimeOffset.UtcNow.AddHours(2));

        PersistenceTestData.BookWorkoutSession(workoutSession, member.Id);

        await using (var arrangeContext = CreateContext())
        {
            arrangeContext.Users.Add(user);
            arrangeContext.Members.Add(member);
            arrangeContext.WorkoutCategories.Add(category);
            arrangeContext.WorkoutTypes.Add(workoutType);
            arrangeContext.WorkoutSessions.Add(workoutSession);
            await arrangeContext.SaveChangesAsync(CancellationToken.None);
        }

        await using var assertContext = CreateContext();
        var sut = new MyAccountBookingRepository(assertContext);

        var result = await sut.GetWorkoutSessionByIdForBookingAsync(workoutSession.Id.Value, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Single(result.Bookings);
    }

    [Fact]
    public async Task GetWorkoutSessionByIdForBookingAsync_ShouldReturnNull_WhenSessionDoesNotExist()
    {
        await using var context = CreateContext();
        var sut = new MyAccountBookingRepository(context);

        var result = await sut.GetWorkoutSessionByIdForBookingAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }
}
