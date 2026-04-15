using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Domain.Aggregates.WorkoutSessions.Services;
using CoreFitness.Domain.Tests.Common;

namespace CoreFitness.Domain.Tests.Aggregates.WorkoutSessions.Services;

public sealed class WorkoutBookingDomainServiceTests
{
    [Fact]
    public void Book_ShouldThrow_WhenMemberHasNoActiveMembership()
    {
        var service = new WorkoutBookingDomainService();
        var member = Member.Create("user-without-membership");
        var session = DomainTestData.CreateFutureWorkoutSession();

        ValidationExceptionAssert.ThrowsWithMessage(
            () => service.Book(member, session),
            MemberErrors.MemberHasNoMembership);
    }

    [Fact]
    public void Book_ShouldCreateBooking_WhenInputIsValid()
    {
        var service = new WorkoutBookingDomainService();
        var member = DomainTestData.CreateMemberWithActiveMembership();
        var session = DomainTestData.CreateFutureWorkoutSession();

        service.Book(member, session);

        Assert.Single(session.Bookings);
        Assert.Equal(member.Id, session.Bookings.Single().MemberId);
    }

    [Fact]
    public void Book_ShouldThrow_WhenMemberAlreadyBooked()
    {
        var service = new WorkoutBookingDomainService();
        var member = DomainTestData.CreateMemberWithActiveMembership();
        var session = DomainTestData.CreateFutureWorkoutSession();
        service.Book(member, session);

        ValidationExceptionAssert.ThrowsWithMessage(
            () => service.Book(member, session),
            WorkoutSessionErrors.MemberAlreadyBooked);
    }

    [Fact]
    public void Book_ShouldThrow_WhenSessionIsFull()
    {
        var service = new WorkoutBookingDomainService();
        var session = DomainTestData.CreateFutureWorkoutSession(capacity: 1);
        var firstMember = DomainTestData.CreateMemberWithActiveMembership("user-1");
        var secondMember = DomainTestData.CreateMemberWithActiveMembership("user-2");

        service.Book(firstMember, session);

        ValidationExceptionAssert.ThrowsWithMessage(
            () => service.Book(secondMember, session),
            WorkoutSessionErrors.SessionIsFull);
    }
}
