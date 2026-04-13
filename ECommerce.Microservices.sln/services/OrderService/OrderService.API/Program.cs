using OrderService.Core;
using OrderService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCore();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.MapControllers();
app.Run();