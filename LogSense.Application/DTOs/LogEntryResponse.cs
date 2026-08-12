using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.DTOs
{
    public class LogEntryResponse
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string? CorrelationId { get; set; }

        public string? Exception { get; set; }
    }
}
