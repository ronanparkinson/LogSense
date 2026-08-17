using LogSense.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.Interfaces
{
    public interface IOpenSearchService
    {
        Task IndexLogEntryAsync(LogEntry logEntry);
        Task<List<LogEntry>> SearchLogsAsync(string query);
        Task<List<LogEntry>> SemanticSearchAsync(string query, int resultCount = 5);
    }
}
