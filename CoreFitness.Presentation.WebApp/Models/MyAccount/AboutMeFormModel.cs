using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Presentation.WebApp.Models.MyAccount;

public sealed class AboutMeFormModel : IValidatableObject
{
    [StringLength(50, ErrorMessage = "First name must be between 2 and 50 characters.")]
    public string? FirstName { get; set; }

    [StringLength(50, ErrorMessage = "Last name must be between 2 and 50 characters.")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [RegularExpression(@"^[0-9+\-\s()]*$", ErrorMessage = "Invalid phone number.")]
    public string? PhoneNumber { get; set; }

    public IFormFile? ProfileImage { get; set; }
    public string? ImageUrl { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrWhiteSpace(FirstName) && FirstName.Trim().Length < 2)
        {
            yield return new ValidationResult(
                "First name must be between 2 and 50 characters.",
                [nameof(FirstName)]);
        }

        if (!string.IsNullOrWhiteSpace(LastName) && LastName.Trim().Length < 2)
        {
            yield return new ValidationResult(
                "Last name must be between 2 and 50 characters.",
                [nameof(LastName)]);
        }

        if (ProfileImage is null)
            yield break;

        const long maxBytes = 2 * 1024 * 1024; // 2 MB (same as JS)
        string[] allowedTypes = ["image/jpeg", "image/png", "image/webp"];

        if (ProfileImage.Length > maxBytes)
        {
            yield return new ValidationResult(
                "Image must be 2 MB or smaller.",
                [nameof(ProfileImage)]);
        }

        if (!allowedTypes.Contains(ProfileImage.ContentType))
        {
            yield return new ValidationResult(
                "Only JPG, PNG or WEBP images are allowed.",
                [nameof(ProfileImage)]);
        }
    }
}
