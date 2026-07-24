using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Domain.Entities;

public class LogEntry
{
    public Guid Id { get; set; }

    public DateTime Timestamp { get; set; }

    public string Level { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string? Source { get; set; }

    public string? CorrelationId { get; set; }

    public string? Exception { get; set; }
}
