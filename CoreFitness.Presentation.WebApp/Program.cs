using CoreFitness.Application.Extensions;
using CoreFitness.Infrastructure;
using CoreFitness.Infrastructure.Extensions;
using CoreFitness.Presentation.WebApp.Configurations;
using CoreFitness.Presentation.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


// Alla URL:er som ASP.NET genererar kommer vara små bokstäver. SEO föredrar
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection("SiteSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{

}

// Kör databasinitiering beroende på aktuell miljö
await InfrastructureInitializer.InitializeAsync(app.Services, app.Environment);

app.UseHsts();
app.UseGlobalExceptionHandling();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapStaticAssets();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
// ??
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
