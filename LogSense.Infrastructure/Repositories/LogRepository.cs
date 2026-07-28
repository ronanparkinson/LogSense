using LogSense.Domain.Entities;
using LogSense.Infrastructure.Persistence;
using LogSense.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly LogSenseDbContext _context;

        public LogRepository(LogSenseDbContext context)
        {
            _context = context;
        }

        public async Task AddLogEntryAsync(LogEntry logEntry)
        {
            _context.LogEntries.Add(logEntry);
            await _context.SaveChangesAsync();
        }
    }
}
