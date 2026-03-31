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
            if (user.IsInRole("Admin"))
            {
                // stoppar execution och gör redirect
                context.Result = new RedirectResult("/admin");
                return;
            }

            if (user.IsInRole("Member"))
            {
                context.Result = new RedirectResult("/account");
                return;
            }

            context.Result = new RedirectResult("/");
        }
    }
}

