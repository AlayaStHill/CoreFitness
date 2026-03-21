using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Presentation.WebApp.Models.Forms;

public class ContactForm
{
    [Required(ErrorMessage = "You must enter a first name")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Minimum {2} characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;


    [Required(ErrorMessage = "You must enter a last name")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Minimum {2} characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "You must enter an email address")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number")]
    [RegularExpression(@"^[0-9+\-\s()]*$", ErrorMessage = "Invalid phone number format")]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }


    [Required(ErrorMessage = "You must enter a message")]
    [StringLength(4000, MinimumLength = 5, ErrorMessage = "Minimum {2} characters")]
    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;
}
