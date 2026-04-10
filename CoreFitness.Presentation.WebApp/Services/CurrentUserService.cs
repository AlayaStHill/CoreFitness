using CoreFitness.Application.Abstractions.Authentication;
using System.Security.Claims;

namespace CoreFitness.Presentation.WebApp.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    public string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
