using Core.DomainServices.Repositories;
using Infrastructure.BoardGamesEF;
using Infrastructure.BoardGamesEF.Repositories;
using Infrastructure.SecurityEF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Configure board games database and services.
builder.Services.AddDbContext<BoardGamesContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("BoardGamesConnection") ??
                           throw new InvalidOperationException("BoardGamesConnection is not configured");
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameEventRepository, GameEventRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<ISnackPreferenceRepository, SnackPreferenceRepository>();

// Configure security database and services.
builder.Services.AddDbContext<SecurityContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SecurityConnection") ??
                           throw new InvalidOperationException("SecurityConnection is not configured");
    options.UseSqlServer(connectionString);
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts => opts.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<SecurityContext>()
    .AddDefaultTokenProviders();


if (builder.Environment.IsDevelopment())
{
    // Recompile razor pages on change in development environment.
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}

// Setup middleware
var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=GameEvent}/{action=All}/{id?}");

// Migrate the database.
using (var scope = app.Services.CreateScope())
{
    await using var boardGamesCtx = scope.ServiceProvider.GetRequiredService<BoardGamesContext>();
    await boardGamesCtx.Database.MigrateAsync();

    await using var securityCtx = scope.ServiceProvider.GetRequiredService<SecurityContext>();
    await securityCtx.Database.MigrateAsync();
}

app.Run();