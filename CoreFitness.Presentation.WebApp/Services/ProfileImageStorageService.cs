using CoreFitness.Application.MyAccount;

namespace CoreFitness.Presentation.WebApp.Services;

public sealed class ProfileImageStorageService(IWebHostEnvironment webHostEnvironment) : IProfileImageStorageService
{
    public async Task<string> SaveProfileImageAsync(Stream fileStream, string originalFileName, CancellationToken ct = default)
    {
        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "profiles");
        Directory.CreateDirectory(uploadsFolder);

        string extension = Path.GetExtension(originalFileName).ToLowerInvariant();
        string fileName = $"{Guid.NewGuid():N}{extension}";
        string filePath = Path.Combine(uploadsFolder, fileName);

        await using FileStream output = new(filePath, FileMode.Create);
        await fileStream.CopyToAsync(output, ct);

        return $"/images/profiles/{fileName}";
    }
}