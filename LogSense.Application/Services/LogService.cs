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

        public LogService(ILogRepository logRepository, ILogger<LogService> logger)
        {
            _logRepository = logRepository;
            _logger = logger;
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

            _logger.LogInformation(
                "Log entry {LogEntryId} created successfully from source {Source}",
                logEntry.Id,
                logEntry.Source);
        }

        public async Task<List<LogEntryResponse>> GetAllLogsAsync()
        {
            List<LogEntry> logEntries =
                await _logRepository.GetAllLogEntriesAsync();

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

        public async Task<List<LogEntryResponse>> GetLogEntriesByLevelAsync(string level)
        {
            List<LogEntry> logEntries =
                await _logRepository.GetLogEntriesByLevelAsync(level);

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
    }
}
