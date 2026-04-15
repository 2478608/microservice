using OrderService.Core;
using OrderService.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddStackExchangeRedisCache(options =>
{
    var baseConn = builder.Configuration["Redis:ConnectionString"];
    var password = builder.Configuration["Redis:Password"];

    if (string.IsNullOrWhiteSpace(password))
        throw new InvalidOperationException(
            "Redis password is missing. Set Redis__Password environment variable.");

    options.Configuration = $"{baseConn},password={password}";
});

builder.Services.AddCore();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.MapControllers();
app.Run();