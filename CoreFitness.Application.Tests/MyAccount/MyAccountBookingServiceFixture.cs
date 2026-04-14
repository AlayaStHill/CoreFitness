using CoreFitness.Application.MyAccount;
using CoreFitness.Application.Shared;
using CoreFitness.Domain.Aggregates.WorkoutSessions.Services;
using NSubstitute;

namespace CoreFitness.Application.Tests.MyAccount;

internal sealed class MyAccountBookingServiceFixture
{
    public IMyAccountBookingRepository BookingRepository { get; } = Substitute.For<IMyAccountBookingRepository>();
    public IUnitOfWork UnitOfWork { get; } = Substitute.For<IUnitOfWork>();
    public WorkoutBookingDomainService WorkoutBookingDomainService { get; } = new();

    public MyAccountBookingService CreateSut() => new(BookingRepository, WorkoutBookingDomainService, UnitOfWork);
}
