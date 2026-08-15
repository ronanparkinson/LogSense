using LogSense.Application.DTOs;
using LogSense.Application.Interfaces;
using LogSense.Domain.Entities;
using LogSense.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly ILogger<LogService> _logger;
        private readonly IOpenSearchService _openSearchService;

        public LogService(
            ILogRepository logRepository,
            ILogger<LogService> logger,
            IOpenSearchService openSearchService)
        {
            _logRepository = logRepository;
            _logger = logger;
            _openSearchService = openSearchService;
        }

        public async Task CreateLogAsync(CreateLogEntryRequest createLogEntryRequest)
        {
            _logger.LogInformation(
                "Creating log entry from source {Source} with level {Level}",
                createLogEntryRequest.Source,
                createLogEntryRequest.Level);

            LogEntry logEntry = new LogEntry
            {
                Level = createLogEntryRequest.Level,
                Message = createLogEntryRequest.Message,
                Source = createLogEntryRequest.Source,
                Exception = createLogEntryRequest.Exception,
                Timestamp = DateTime.UtcNow
            };

            await _logRepository.AddLogEntryAsync(logEntry);
            await _openSearchService.IndexLogEntryAsync(logEntry);

            _logger.LogInformation(
                "Log entry {LogEntryId} created successfully from source {Source}",
                logEntry.Id,
                logEntry.Source);
        }

        public async Task<List<LogEntryResponse>> QueryLogsAsync(LogQueryParameters parameters)
        {
            List<LogEntry> logEntries =
                await _logRepository.QueryLogEntriesAsync(parameters);

            return logEntries.Select(logEntry => new LogEntryResponse
            {
                Id = logEntry.Id,
                Timestamp = logEntry.Timestamp,
                Level = logEntry.Level,
                Message = logEntry.Message,
                Source = logEntry.Source,
                CorrelationId = logEntry.CorrelationId,
                Exception = logEntry.Exception
            }).ToList();
        }

        public async Task<List<LogEntryResponse>> SearchLogsAsync(string query)
        {
            var logs = await _openSearchService.SearchLogsAsync(query);

            return logs.Select(log => new LogEntryResponse
            {
                Id = log.Id,
                Timestamp = log.Timestamp,
                Level = log.Level,
                Message = log.Message,
                Source = log.Source,
                CorrelationId = log.CorrelationId,
                Exception = log.Exception
            }).ToList();
        }
    }
}
