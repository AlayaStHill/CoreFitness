using CoreFitness.Application.Extensions;
using CoreFitness.Infrastructure;
using CoreFitness.Infrastructure.Extensions;
using CoreFitness.Presentation.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();

// Alla URL:er som ASP.NET genererar kommer vara små bokstäver. SEO föredrar
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

// Kör databasinitiering beroende på aktuell miljö
await PersistenceDatabaseInitializer.InitializeAsync(app.Services, app.Environment);

app.UseHsts();
app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
