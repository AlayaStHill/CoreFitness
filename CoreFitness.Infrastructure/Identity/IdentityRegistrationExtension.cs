using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Infrastructure.Identity.Models;
using CoreFitness.Infrastructure.Identity.Services;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Infrastructure.Identity;

public static class IdentityRegistrationExtension
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        // konfigurera systemet
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = configuration.GetValue<bool>("IdentitySettings:RequireUniqueEmail");

            options.Password.RequiredLength = configuration.GetValue<int>("IdentitySettings:MinPasswordLength");

            options.SignIn.RequireConfirmedAccount = configuration.GetValue<bool>("IdentitySettings:RequireConfirmedAccount");
            options.SignIn.RequireConfirmedPhoneNumber = configuration.GetValue<bool>("IdentitySettings:RequireConfirmedPhoneNumber");
            options.SignIn.RequireConfirmedEmail = configuration.GetValue<bool>("IdentitySettings:RequireConfirmedEmail");

        })
        // databaskoppling som ska användas för att skapa Identity-tabeller 
        .AddEntityFrameworkStores<PersistenceContext>()
        // tokens för ex. autentisering
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = configuration.GetValue<string>("AppCookieSettings:LoginPath") ?? "/Identity/Account/Login";
            options.LogoutPath = configuration.GetValue<string>("AppCookieSettings:LogoutPath") ?? "/Identity/Account/Logout";
            options.AccessDeniedPath = configuration.GetValue<string>("AppCookieSettings:AccessDeniedPath") ?? "/Identity/Account/AccessDenied";

            options.Cookie.Name = configuration.GetValue<string>("AppCookieSettings:CookieName") ?? "CoreFitnessAppCookie";
            options.ExpireTimeSpan = TimeSpan.FromDays(configuration.GetValue<int>("AppCookieSettings:ExpiresInDays"));
            options.SlidingExpiration = true;
        });

        services.AddScoped<IAuthService, IdentityAuthService>();

        return services;
    }
}