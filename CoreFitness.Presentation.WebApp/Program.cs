using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Application.Extensions;
using CoreFitness.Application.MyAccount;
using CoreFitness.Infrastructure;
using CoreFitness.Infrastructure.Extensions;
using CoreFitness.Presentation.WebApp.Configurations;
using CoreFitness.Presentation.WebApp.Extensions;
using CoreFitness.Presentation.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IProfileImageStorageService, ProfileImageStorageService>();



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

// kopplar om alla 404-fel till en anpassad error-sida, där {0} kommer att ersättas med den faktiska statuskoden 
app.UseStatusCodePagesWithReExecute("/Error/{0}");

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
