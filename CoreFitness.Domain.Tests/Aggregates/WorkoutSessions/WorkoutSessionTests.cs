using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Domain.Aggregates.WorkoutTypes;
using CoreFitness.Domain.Tests.Common;

namespace CoreFitness.Domain.Tests.Aggregates.WorkoutSessions;

public sealed class WorkoutSessionTests
{
    [Fact]
    public void Create_ShouldThrow_WhenStartsAtIsInThePast()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => WorkoutSession.Create(
                DateTimeOffset.UtcNow.AddMinutes(-1),
                CoreFitness.Domain.Aggregates.WorkoutTypes.WorkoutTypeId.Create(),
                TimeSpan.FromMinutes(45),
                10),
            WorkoutSessionErrors.InvalidStartsAt);
    }

    [Fact]
    public void Create_ShouldThrow_WhenCapacityIsZero()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => WorkoutSession.Create(
                DateTimeOffset.UtcNow.AddHours(1),
                WorkoutTypeId.Create(),
                TimeSpan.FromMinutes(45),
                0),
            WorkoutSessionErrors.InvalidCapacity);
    }

    [Fact]
    public void Create_ShouldThrow_WhenDurationIsZero()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => WorkoutSession.Create(
                DateTimeOffset.UtcNow.AddHours(1),
                WorkoutTypeId.Create(),
                TimeSpan.Zero,
                10),
            WorkoutSessionErrors.InvalidDuration);
    }

    [Fact]
    public void Create_ShouldThrow_WhenWorkoutTypeIdIsEmpty()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => WorkoutSession.Create(
                DateTimeOffset.UtcNow.AddHours(1),
                new WorkoutTypeId(Guid.Empty),
                TimeSpan.FromMinutes(45),
                10),
            WorkoutSessionErrors.WorkoutTypeIdRequired);
    }

    [Fact]
    public void HasAvailableSpots_ShouldBeTrue_WhenNoBookingsExist()
    {
        var session = DomainTestData.CreateFutureWorkoutSession(capacity: 1);

        Assert.True(session.HasAvailableSpots());
        Assert.Equal(1, session.AvailableSpots());
    }

    [Fact]
    public void CancelBooking_ShouldThrow_WhenBookingDoesNotExist()
    {
        var session = DomainTestData.CreateFutureWorkoutSession();

        ValidationExceptionAssert.ThrowsWithMessage(
            () => session.CancelBooking(MemberId.Create()),
            WorkoutSessionErrors.BookingNotFound);
    }

    [Fact]
    public void CancelBooking_ShouldThrow_WhenMemberIdIsEmpty()
    {
        var session = DomainTestData.CreateFutureWorkoutSession();

        ValidationExceptionAssert.ThrowsWithMessage(
            () => session.CancelBooking(new MemberId(Guid.Empty)),
            WorkoutSessionErrors.MemberIdRequired);
    }

    [Fact]
    public void CancelBooking_ShouldRemoveBooking_WhenBookingExists()
    {
        var service = new CoreFitness.Domain.Aggregates.WorkoutSessions.Services.WorkoutBookingDomainService();
        var session = DomainTestData.CreateFutureWorkoutSession();
        var member = DomainTestData.CreateMemberWithActiveMembership();

        service.Book(member, session);
        session.CancelBooking(member.Id);

        Assert.Empty(session.Bookings);
        Assert.Equal(session.Capacity, session.AvailableSpots());
    }

    [Fact]
    public void HasAvailableSpots_ShouldBeFalse_WhenSessionIsFull()
    {
        var service = new CoreFitness.Domain.Aggregates.WorkoutSessions.Services.WorkoutBookingDomainService();
        var session = DomainTestData.CreateFutureWorkoutSession(capacity: 1);
        var member = DomainTestData.CreateMemberWithActiveMembership();

        service.Book(member, session);

        Assert.False(session.HasAvailableSpots());
        Assert.Equal(0, session.AvailableSpots());
    }
}
