namespace CoreFitness.Application.MyAccount;

public interface IProfileImageStorageService
{
    Task<string> SaveProfileImageAsync(Stream fileStream, string originalFileName, CancellationToken ct = default);
}
