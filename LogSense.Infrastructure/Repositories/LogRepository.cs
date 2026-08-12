using LogSense.Domain.Entities;
using LogSense.Infrastructure.Persistence;
using LogSense.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<LogEntry>> GetAllLogEntriesAsync()
        {
            return await _context.LogEntries.ToListAsync();
        }

        public async Task<List<LogEntry>> GetLogEntriesByLevelAsync(string level)
        {
            return await _context.LogEntries.Where(LogEntry => LogEntry.Level == level).ToListAsync();
        }

        public async Task<List<LogEntry>> GetLogEntriesBySourceAsync(string source)
        {
            return await _context.LogEntries
                .Where(logEntry => logEntry.Source == source)
                .ToListAsync();
        }

    }
}
