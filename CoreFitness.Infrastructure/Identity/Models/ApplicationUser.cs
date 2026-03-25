using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between {2} and {1} characters")]
    public string? FirstName { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between {2} and {1} characters")]
    public string? LastName { get; set; }

    public string? ImageUrl { get; set; }

}
