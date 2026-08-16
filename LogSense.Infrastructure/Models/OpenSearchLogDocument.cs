using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Infrastructure.Models
{
    public class OpenSearchLogDocument
    {
        public Guid Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Level { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public string? CorrelationId { get; set; }

        public string? Exception { get; set; }

        public float[] Embedding { get; set; } = [];
    }
}
