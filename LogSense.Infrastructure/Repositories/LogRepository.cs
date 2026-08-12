using LogSense.Application.DTOs;
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

        public async Task<List<LogEntry>> QueryLogEntriesAsync(LogQueryParameters parameters)
        {
            IQueryable<LogEntry> query = _context.LogEntries;

            if (!string.IsNullOrWhiteSpace(parameters.Level))
            {
                query = query.Where(logEntry =>
                    logEntry.Level == parameters.Level);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Source))
            {
                query = query.Where(logEntry =>
                    logEntry.Source == parameters.Source);
            }

            if (parameters.From.HasValue)
            {
                query = query.Where(logEntry =>
                    logEntry.Timestamp >= parameters.From.Value);
            }

            if (parameters.To.HasValue)
            {
                query = query.Where(logEntry =>
                    logEntry.Timestamp <= parameters.To.Value);
            }

            query = query.OrderByDescending(logEntry =>
                logEntry.Timestamp);

            query = query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize);

            return await query.ToListAsync();
        }

    }
}
