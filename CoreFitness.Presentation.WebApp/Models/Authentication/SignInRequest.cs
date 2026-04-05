using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Presentation.WebApp.Models.Authentication;

public sealed class SignInRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    [Display(Name = "Email Address *", Prompt = "username@example.com")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password *", Prompt = "Enter Password")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; } = false;
}
