using SpotifyExtension.DataItems.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("app.json")
    .AddJsonFile($"app.{builder.Environment.EnvironmentName}.json");

builder.Services.Configure<OAuthOptions>(builder.Configuration.GetSection("OAuth"));
builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection("Application"));

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
