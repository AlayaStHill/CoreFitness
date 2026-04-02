using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Presentation.WebApp.Models.Authentication;

public class SetPasswordRequest
{
    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(
     @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
     ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character"
 )]
    [DataType(DataType.Password)]
    [Display(Name = "Password *", Prompt = "Enter Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password must be confirmed")]
    [Compare(nameof(Password), ErrorMessage = "Password must match")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password *", Prompt = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms and conditions")]
    [Display(Name = "Accept user terms and conditions")]
    public bool TermsAndConditions { get; set; }
}

