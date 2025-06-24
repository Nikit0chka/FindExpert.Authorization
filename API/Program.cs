using API.Extensions;
using API.Middlewares;
using Application.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using HttpClients.Extensions;
using Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

var loggerFactory = LoggerFactory.Create(static loggingBuilder => { loggingBuilder.AddSerilog(); });

var logger = loggerFactory.CreateLogger<Program>();

builder.Services.AddApiServices(logger);
builder.Services.AddInfrastructureServices(builder.Configuration, logger);
builder.Services.AddApplicationServices(logger);
builder.Services.AddHttpClients(logger);

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseFastEndpoints().UseSwaggerGen();

app.Run();