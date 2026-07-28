using LogSense.Application.DTOs;
using LogSense.Application.Interfaces;
using LogSense.Domain.Entities;
using LogSense.Infrastructure.Repositories.Interfaces;
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

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task CreateLogAsync(CreateLogEntryRequest createLogEntryRequest)
        {
            LogEntry logEntry = new LogEntry
            {
                Level = createLogEntryRequest.Level,
                Message = createLogEntryRequest.Message,
                Source = createLogEntryRequest.Source,
                Exception = createLogEntryRequest.Exception,
                Timestamp = DateTime.UtcNow
            };

            await _logRepository.AddLogEntryAsync(logEntry);
        }
    }
}
