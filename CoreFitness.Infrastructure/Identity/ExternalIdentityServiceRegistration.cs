using AspNet.Security.OAuth.GitHub;
using CoreFitness.Infrastructure.Identity.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace CoreFitness.Infrastructure.Identity;

public static class ExternalIdentityServiceRegistration
{
    public static IServiceCollection AddExternalIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        
        AuthenticationBuilder authenticationBuilder = services.AddAuthentication();
        // hämtar från secrets.json (eller appsettings.json), där clientid och secret för github ligger - mappar till objektet GitHubAuthOptions
        var gitHubOptions = configuration.GetSection(GitHubAuthOptions.SectionName)
        .Get<GitHubAuthOptions>();

        if (gitHubOptions is not null && !string.IsNullOrWhiteSpace(gitHubOptions.ClientId) && !string.IsNullOrWhiteSpace(gitHubOptions.ClientSecret))
        {
            authenticationBuilder.AddGitHub(GitHubAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = gitHubOptions.ClientId;
                options.ClientSecret = gitHubOptions.ClientSecret;
                options.SignInScheme = IdentityConstants.ExternalScheme;
                // URL dit GitHub skickar användaren efter login
                options.CallbackPath = "/signin-github";

                // hämta med e-postadressen för användaren
                options.Scope.Add("user:email");

                // Id i objektet github skickar mappas om till våran NameIdentifier --> Id = NameIdentifier, etc
                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");

                // de skickar avatar url, vi sparar in det som urn:github::avatar etc - populerar våra claimsbitar
                options.ClaimActions.MapJsonKey("urn:github:login", "login");
                options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
                options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

                options.SaveTokens = true;
            });
        }

        return services;
    }
}

