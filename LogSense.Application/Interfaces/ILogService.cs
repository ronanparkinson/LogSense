using LogSense.Application.DTOs;
using LogSense.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.Interfaces
{
    public interface ILogService
    {
        Task CreateLogAsync(CreateLogEntryRequest createLogEntryRequest);
        Task <List<LogEntryResponse>> GetAllLogsAsync();
        Task<List<LogEntryResponse>> GetLogEntriesByLevelAsync(string level);
        Task<List<LogEntryResponse>> GetLogEntriesBySourceAsync(string source);

    }
}
