using LogSense.Api.Middleware;
using LogSense.Application.Interfaces;
using LogSense.Application.Services;
using LogSense.Infrastructure.Persistence;
using LogSense.Infrastructure.Repositories.Interfaces;
using LogSense.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using OpenSearch.Client;
using LogSense.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Register Entity Framework Core
builder.Services.AddDbContext<LogSenseDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("LogSenseDb")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

var openSearchUri =
    builder.Configuration["OpenSearch:Uri"];

var defaultIndex =
    builder.Configuration["OpenSearch:DefaultIndex"];

var settings = new ConnectionSettings(new Uri(openSearchUri!))
    .DefaultIndex(defaultIndex);

var openSearchClient = new OpenSearchClient(settings);

builder.Services.AddSingleton<IOpenSearchClient>(openSearchClient);

builder.Services.AddScoped<IOpenSearchService, OpenSearchService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();