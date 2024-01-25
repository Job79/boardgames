using System.Text;
using BoardGamesApi.GraphQL;
using Core.DomainServices.Repositories;
using HotChocolate.AspNetCore;
using Infrastructure.BoardGamesEF;
using Infrastructure.BoardGamesEF.Repositories;
using Infrastructure.SecurityEF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

// Configure board games database and services.
builder.Services.AddDbContext<BoardGamesContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("BoardGamesConnection") ??
                           throw new InvalidOperationException("BoardGamesConnection is not configured");
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<BoardGamesContext>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameEventRepository, GameEventRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();

// Configure security database and services.
builder.Services.AddDbContext<SecurityContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SecurityConnection") ??
                           throw new InvalidOperationException("SecurityConnection is not configured");
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(opts => opts.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<SecurityContext>();

// Setup JWT authentication.
builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ??
                                   throw new InvalidOperationException("Jwt:Key is not configured")))
    }
);

// Add swagger authentication input field.
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "BoardGamesAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header, Description = "Please enter token", Name = "Authorization",
            Type = SecuritySchemeType.Http, BearerFormat = "JWT", Scheme = "bearer"
        });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
                { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            new string[] { }
        }
    });
});

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapGraphQL();
app.UsePlayground();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

// Migrate the database.
using (var scope = app.Services.CreateScope())
{
    await using var boardGamesCtx = scope.ServiceProvider.GetRequiredService<BoardGamesContext>();
    await boardGamesCtx.Database.MigrateAsync();

    await using var securityCtx = scope.ServiceProvider.GetRequiredService<SecurityContext>();
    await securityCtx.Database.MigrateAsync();
}

app.Run();