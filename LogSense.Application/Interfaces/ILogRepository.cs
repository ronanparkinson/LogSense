using LogSense.Application.DTOs;
using LogSense.Domain.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Infrastructure.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task AddLogEntryAsync(LogEntry logEntry);
        Task<List<LogEntry>> QueryLogEntriesAsync(LogQueryParameters parameters);
    }
}
