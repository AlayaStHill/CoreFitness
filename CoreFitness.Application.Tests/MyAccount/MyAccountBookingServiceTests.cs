using CoreFitness.Application.MyAccount;
using CoreFitness.Application.Tests.Common;
using NSubstitute;

namespace CoreFitness.Application.Tests.MyAccount;

public sealed class MyAccountBookingServiceTests
{
    [Fact]
    public async Task GetOverviewAsync_ShouldReturnNull_WhenUserIdIsMissing()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.GetOverviewAsync(" ");

        Assert.Null(result);
        await fixture.BookingRepository.DidNotReceive().GetByUserIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetOverviewAsync_ShouldSplitUpcomingAndPreviousBookings()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();
        var now = DateTimeOffset.UtcNow;
        var bookings = new List<CoreFitness.Application.MyAccount.Outputs.MyBookingItemOutput>
        {
            ApplicationTestData.CreateMyBookingItemOutput(now.AddHours(2)),
            ApplicationTestData.CreateMyBookingItemOutput(now.AddHours(-3)),
            ApplicationTestData.CreateMyBookingItemOutput(now.AddHours(1)),
            ApplicationTestData.CreateMyBookingItemOutput(now.AddHours(-1))
        };

        fixture.BookingRepository.GetByUserIdAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CoreFitness.Application.MyAccount.Outputs.MyBookingItemOutput>>(bookings));

        var result = await sut.GetOverviewAsync("user-1");

        Assert.NotNull(result);
        Assert.Equal(2, result.UpcomingBookings.Count);
        Assert.Equal(2, result.PreviousBookings.Count);
        Assert.True(result.UpcomingBookings[0].StartsAt <= result.UpcomingBookings[1].StartsAt);
        Assert.True(result.PreviousBookings[0].StartsAt >= result.PreviousBookings[1].StartsAt);
    }

    [Fact]
    public async Task GetUpcomingSessionsAsync_ShouldReturnEmpty_WhenUserIdIsMissing()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.GetUpcomingSessionsAsync(" ");

        Assert.Empty(result);
        await fixture.BookingRepository.DidNotReceive().GetUpcomingSessionsByUserIdAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetUpcomingSessionsAsync_ShouldReturnRepositoryValue_WhenUserIdIsValid()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();
        var sessions = new List<CoreFitness.Application.MyAccount.Outputs.MyUpcomingSessionOutput>
        {
            ApplicationTestData.CreateMyUpcomingSessionOutput(DateTimeOffset.UtcNow.AddHours(1))
        };

        fixture.BookingRepository.GetUpcomingSessionsByUserIdAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CoreFitness.Application.MyAccount.Outputs.MyUpcomingSessionOutput>>(sessions));

        var result = await sut.GetUpcomingSessionsAsync("user-1");

        Assert.Single(result);
    }

    [Fact]
    public async Task BookSessionAsync_ShouldFail_WhenUserIdIsMissing()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.BookSessionAsync(" ", Guid.NewGuid());

        ResultAssert.Failure(result, MyAccountErrors.UserIdRequired);
    }

    [Fact]
    public async Task BookSessionAsync_ShouldFail_WhenWorkoutSessionIdIsEmpty()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.BookSessionAsync("user-1", Guid.Empty);

        ResultAssert.Failure(result, MyAccountErrors.WorkoutSessionIdRequired);
    }

    [Fact]
    public async Task BookSessionAsync_ShouldFail_WhenMemberIsNotFound()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();

        fixture.BookingRepository.GetMemberByUserIdForBookingAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(null));

        var result = await sut.BookSessionAsync("user-1", Guid.NewGuid());

        ResultAssert.Failure(result, MyAccountErrors.MemberNotFound);
    }

    [Fact]
    public async Task BookSessionAsync_ShouldFail_WhenWorkoutSessionIsNotFound()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMemberWithActiveMembership();
        var sessionId = Guid.NewGuid();

        fixture.BookingRepository.GetMemberByUserIdForBookingAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.BookingRepository.GetWorkoutSessionByIdForBookingAsync(sessionId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.WorkoutSessions.WorkoutSession?>(null));

        var result = await sut.BookSessionAsync("user-1", sessionId);

        ResultAssert.Failure(result, MyAccountErrors.WorkoutSessionNotFound);
    }

    [Fact]
    public async Task BookSessionAsync_ShouldFailWithValidation_WhenDomainValidationFails()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMember();
        var session = ApplicationTestData.CreateFutureWorkoutSession();
        var sessionId = session.Id.Value;

        fixture.BookingRepository.GetMemberByUserIdForBookingAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.BookingRepository.GetWorkoutSessionByIdForBookingAsync(sessionId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.WorkoutSessions.WorkoutSession?>(session));

        var result = await sut.BookSessionAsync("user-1", sessionId);

        Assert.True(result.IsFailure);
        Assert.Equal(CoreFitness.Application.Shared.Results.ErrorTypes.BadRequest, result.Error!.Type);
        await fixture.UnitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task BookSessionAsync_ShouldSave_WhenInputIsValid()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMemberWithActiveMembership();
        var session = ApplicationTestData.CreateFutureWorkoutSession();
        var sessionId = session.Id.Value;

        fixture.BookingRepository.GetMemberByUserIdForBookingAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.BookingRepository.GetWorkoutSessionByIdForBookingAsync(sessionId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.WorkoutSessions.WorkoutSession?>(session));
        fixture.UnitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        var result = await sut.BookSessionAsync("user-1", sessionId);

        ResultAssert.Success(result);
        await fixture.UnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CancelSessionAsync_ShouldFail_WhenUserIdIsMissing()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.CancelSessionAsync(" ", Guid.NewGuid());

        ResultAssert.Failure(result, MyAccountErrors.UserIdRequired);
    }

    [Fact]
    public async Task CancelSessionAsync_ShouldFail_WhenWorkoutSessionIdIsEmpty()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.CancelSessionAsync("user-1", Guid.Empty);

        ResultAssert.Failure(result, MyAccountErrors.WorkoutSessionIdRequired);
    }

    [Fact]
    public async Task CancelSessionAsync_ShouldFail_WhenMemberIsNotFound()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();

        fixture.BookingRepository.GetMemberByUserIdForBookingAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(null));

        var result = await sut.CancelSessionAsync("user-1", Guid.NewGuid());

        ResultAssert.Failure(result, MyAccountErrors.MemberNotFound);
    }

    [Fact]
    public async Task CancelSessionAsync_ShouldFail_WhenWorkoutSessionIsNotFound()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMemberWithActiveMembership();
        var sessionId = Guid.NewGuid();

        fixture.BookingRepository.GetMemberByUserIdForBookingAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.BookingRepository.GetWorkoutSessionByIdForBookingAsync(sessionId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.WorkoutSessions.WorkoutSession?>(null));

        var result = await sut.CancelSessionAsync("user-1", sessionId);

        ResultAssert.Failure(result, MyAccountErrors.WorkoutSessionNotFound);
    }

    [Fact]
    public async Task CancelSessionAsync_ShouldFailWithValidation_WhenBookingIsMissing()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMemberWithActiveMembership();
        var session = ApplicationTestData.CreateFutureWorkoutSession();
        var sessionId = session.Id.Value;

        fixture.BookingRepository.GetMemberByUserIdForBookingAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.BookingRepository.GetWorkoutSessionByIdForBookingAsync(sessionId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.WorkoutSessions.WorkoutSession?>(session));

        var result = await sut.CancelSessionAsync("user-1", sessionId);

        Assert.True(result.IsFailure);
        Assert.Equal(CoreFitness.Application.Shared.Results.ErrorTypes.BadRequest, result.Error!.Type);
        await fixture.UnitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CancelSessionAsync_ShouldSave_WhenBookingExists()
    {
        var fixture = new MyAccountBookingServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMemberWithActiveMembership();
        var session = ApplicationTestData.CreateFutureWorkoutSession();
        var sessionId = session.Id.Value;

        fixture.BookingRepository.GetMemberByUserIdForBookingAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.BookingRepository.GetWorkoutSessionByIdForBookingAsync(sessionId, Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.WorkoutSessions.WorkoutSession?>(session));
        fixture.UnitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        await sut.BookSessionAsync("user-1", sessionId);
        var result = await sut.CancelSessionAsync("user-1", sessionId);

        ResultAssert.Success(result);
        await fixture.UnitOfWork.Received(2).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
