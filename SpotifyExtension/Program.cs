using SpotifyExtension.DataItems.Config;
using SpotifyExtension.Interfaces.Services;
using SpotifyExtension.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("app.json")
    .AddJsonFile($"app.{builder.Environment.EnvironmentName}.json");

builder.Services.Configure<OAuthOptions>(builder.Configuration.GetSection("OAuth"));
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection("Application"));

builder.Services.AddTransient<IAuthorizeService, AuthorizeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
