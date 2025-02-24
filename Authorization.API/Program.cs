using Authorization.API.Extensions;
using Authorization.API.Middlewares;
using Authorization.Application.Extensions;
using Authorization.Infrastructure.Extensions;
using FastEndpoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var loggerFactory = LoggerFactory.Create(static loggingBuilder =>
{
    loggingBuilder.AddSerilog(); // Интеграция Serilog
});

var logger = loggerFactory.CreateLogger("Program");

builder.Services.AddApiServices(logger);
builder.Services.AddInfrastructureServices(builder.Configuration, logger);
builder.Services.AddApplicationServices(logger);

var app = builder.Build();

app.UseSwagger();
app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(static c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseFastEndpoints();
app.Run();