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
using Microsoft.Extensions.AI;
using OllamaSharp;

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

var ollamaEndpoint =
    builder.Configuration["Ollama:Endpoint"];

var ollamaModel =
    builder.Configuration["Ollama:ChatModel"];

var ollamaEmbeddingModel =
    builder.Configuration["Ollama:EmbeddingModel"];

IChatClient chatClient = new OllamaApiClient(
    new Uri(ollamaEndpoint!),
    ollamaModel!);

builder.Services.AddSingleton<IChatClient>(chatClient);

IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
    new OllamaApiClient(
        new Uri(ollamaEndpoint!),
        ollamaEmbeddingModel!);

builder.Services.AddSingleton<
    IEmbeddingGenerator<string, Embedding<float>>>(
        embeddingGenerator);

builder.Services.AddScoped<
    ILogEmbeddingService,
    LogEmbeddingService>();

builder.Services.AddScoped<ILogAnalysisService, LogAnalysisService>();

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