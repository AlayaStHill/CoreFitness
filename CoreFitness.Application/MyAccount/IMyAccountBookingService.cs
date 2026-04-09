using CoreFitness.Application.MyAccount.Outputs;

namespace CoreFitness.Application.MyAccount;

public interface IMyAccountBookingService
{
    Task<MyBookingsOverviewOutput?> GetOverviewAsync(string userId, CancellationToken ct = default);
}