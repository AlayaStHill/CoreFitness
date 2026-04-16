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


    public static ApplicationUser Create(string email)
    {
        return new ApplicationUser
        {
            UserName = email.Trim().ToLowerInvariant(),
            Email = email.Trim().ToLowerInvariant(),
        };
    }

    // phonenumber från IdentityUser
    public static ApplicationUser UpdateDetails(ApplicationUser user, string? firstName, string? lastName, string? imageUrl, string? phoneNumber)
    {
        if (user.FirstName != firstName)
            user.FirstName = firstName;

        if (user.LastName != lastName)
            user.LastName = lastName;

        if (user.ImageUrl != imageUrl)
            user.ImageUrl = imageUrl;

        if (user.PhoneNumber != phoneNumber)
            user.PhoneNumber = phoneNumber;
        
        return user;
    }
}

