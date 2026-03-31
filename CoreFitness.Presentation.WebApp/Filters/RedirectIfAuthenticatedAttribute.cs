using CoreFitness.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CoreFitness.Presentation.WebApp.Filters;

public sealed class RedirectIfAuthenticatedAttribute : ActionFilterAttribute
{
    // körs innan Controller-action
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // en representation av användaren
        ClaimsPrincipal user = context.HttpContext.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            if (user.IsInRole(Roles.Admin))
            {
                // stoppar execution och gör redirect, action: Index, controller: Admin, null = route values (extra data som skickas till routingen (URL:en))
                context.Result = new RedirectToActionResult("Index", "Admin", null);
                return;
            }

            if (user.IsInRole(Roles.Member))
            {
                context.Result = new RedirectToActionResult("Index", "Account", null);
                return;
            }

            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}



