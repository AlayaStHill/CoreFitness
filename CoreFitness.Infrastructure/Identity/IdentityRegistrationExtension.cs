using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Application.MyAccount;
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

        // Används för autentisering. Konfigurerar hur claims lagras och skickas mellan requests. När en användare loggar in, skapas en cookie som innehåller information om användarens identitet och roller (claims). Denna cookie skickas sedan med varje efterföljande request, vilket gör att systemet kan identifiera användaren och ge tillgång till resurser baserat på deras roller och behörigheter.
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = configuration.GetValue<string>("AppCookieSettings:LoginPath") ?? "/Identity/Account/Login";
            options.LogoutPath = configuration.GetValue<string>("AppCookieSettings:LogoutPath") ?? "/Identity/Account/Logout";
            options.AccessDeniedPath = configuration.GetValue<string>("AppCookieSettings:AccessDeniedPath") ?? "/Identity/Account/AccessDenied";

            options.Cookie.Name = configuration.GetValue<string>("AppCookieSettings:CookieName") ?? "CoreFitnessAppCookie";
            options.ExpireTimeSpan = TimeSpan.FromDays(configuration.GetValue<int>("AppCookieSettings:ExpiresInDays"));
            options.SlidingExpiration = true;
            // sätter nödvändiga cookies för att hemsidan ska fungera korrekt - essential-delar som rör exempelvis inloggning - dessa blir inte valbara
            options.Cookie.IsEssential = true;
        });

        services.AddScoped<IAuthService, IdentityAuthService>();
        services.AddScoped<IMyAccountUserService, MyAccountUserService>();

        services.AddExternalIdentity(configuration);

        return services;
    }
}