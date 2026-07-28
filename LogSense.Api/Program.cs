using LogSense.Api.Middleware;
using LogSense.Application.Interfaces;
using LogSense.Application.Services;
using LogSense.Infrastructure.Persistence;
using LogSense.Infrastructure.Repositories.Interfaces;
using LogSense.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

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