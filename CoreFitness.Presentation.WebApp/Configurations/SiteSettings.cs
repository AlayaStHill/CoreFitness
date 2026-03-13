namespace CoreFitness.Presentation.WebApp.Configurations;

public sealed class SiteSettings
{
    public required string SiteName { get; init; } = "en";
    public required string Slogan { get; init; } = "CoreFitness";
    public required string Description { get; init; } = "";
    public required string Language { get; init; } = "";
    public required string Copyright { get; init; } = "";
}
