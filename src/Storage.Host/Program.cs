using Storage.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Services.AddStorage(builder.Configuration);
builder.Services.AddMinio(builder.Configuration);

var app = builder.Build();

app.UseDokan();

await app.RunAsync();
