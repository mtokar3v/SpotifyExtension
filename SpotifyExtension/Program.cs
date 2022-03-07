using SpotifyExtension.DataItems.Config;
using SpotifyExtension.Interfaces.Repository;
using SpotifyExtension.Interfaces.Services;
using SpotifyExtension.Repositoty;
using SpotifyExtension.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("app.json")
    .AddJsonFile($"app.{builder.Environment.EnvironmentName}.json");

builder.Services.Configure<OAuthOptions>(builder.Configuration.GetSection("OAuth"));
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection("Application"));
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddTransient<ISpotifyTracksRepository, SpotifyTracksRepository>();

builder.Services.AddTransient<IAuthorizeService, AuthorizeService>();
builder.Services.AddSingleton<ICookieService, CookieService>();
builder.Services.AddSingleton<ISessionService, SessionService>();

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

app.MapControllers();

app.Run();
