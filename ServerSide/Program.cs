using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.IdentityModel.Tokens;
using SpotifyExtension.DataItems.Config;
using SpotifyExtension.DataItems.Options;
using SpotifyExtension.Interfaces.Repository;
using SpotifyExtension.Interfaces.Services;
using SpotifyExtension.Repositoty;
using SpotifyExtension.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("app.json");

if (!string.IsNullOrEmpty(builder.Environment.EnvironmentName))
{
    builder.Configuration.AddJsonFile($"app.{builder.Environment.EnvironmentName}.json", optional: true);
}

builder.Services.Configure<OAuthOptions>(builder.Configuration.GetSection("OAuth"));
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection("Application"));
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddTransient<ISpotifyTracksRepository, SpotifyTracksRepository>();

builder.Services.AddTransient<IPlayerService, PlayerService>();
builder.Services.AddTransient<IAuthorizeService, AuthService>();
builder.Services.AddSingleton<ICookieService, CookieService>();
builder.Services.AddSingleton<ISessionService, SessionService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = builder.Environment.EnvironmentName == "Development";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = JwtAuthOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = JwtAuthOptions.Audience,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = JwtAuthOptions.GetSymmetricSecurityKey()
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
