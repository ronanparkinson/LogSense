using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LogSense.Domain.Entities;

namespace LogSense.Infrastructure.Persistence;

public class LogSenseDbContext : DbContext
{
    public LogSenseDbContext(DbContextOptions<LogSenseDbContext> options)
        : base(options)
    {
    }

    public DbSet<LogEntry> LogEntries => Set<LogEntry>();
}
