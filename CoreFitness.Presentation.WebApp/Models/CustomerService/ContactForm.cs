using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Presentation.WebApp.Models.CustomerService;

public class ContactForm
{
    // string.empty kopplatt till AddModelError i controller, så att det inte blir null och därmed inte kraschar när man försöker visa valideringsfel i vyn
    [Required(ErrorMessage = "You must enter a first name")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Must be between {2} and {1} characters")]
    [Display(Name = "First Name", Prompt = "Enter First Name")]
    public string FirstName { get; set; } = string.Empty;


    [Required(ErrorMessage = "You must enter a last name")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Must be between {2} and {1} characters")]
    [Display(Name = "Last Name", Prompt = "Enter Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "You must enter an email address")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format")]
    [Display(Name = "Email Address", Prompt = "username@example.com")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number")]
    [RegularExpression(@"^[0-9+\-\s()]*$", ErrorMessage = "Invalid phone number format")]
    [Display(Name = "Phone Number", Prompt = "Enter Phone Number")]
    public string? PhoneNumber { get; set; }


    [Required(ErrorMessage = "You must enter a message")]
    [StringLength(2000, MinimumLength = 5, ErrorMessage = "Must be between {2} and {1} characters")]
    [Display(Name = "Message", Prompt = "Message...")]
    public string Message { get; set; } = string.Empty;
}
