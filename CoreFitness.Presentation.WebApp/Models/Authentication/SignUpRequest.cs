using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Presentation.WebApp.Models.Authentication;

public class SignUpRequest()
{
    [Required(ErrorMessage = "You must enter an email address")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format")]
    [Display(Name = "Email Address", Prompt = "username@example.com")]
    public string Email { get; set; } = string.Empty;
}

