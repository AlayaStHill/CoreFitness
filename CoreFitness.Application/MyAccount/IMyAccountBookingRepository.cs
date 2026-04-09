using CoreFitness.Application.MyAccount.Outputs;

namespace CoreFitness.Application.MyAccount;

public interface IMyAccountBookingRepository
{
    Task<IReadOnlyList<MyBookingItemOutput>> GetByUserIdAsync(string userId, CancellationToken ct = default);
}