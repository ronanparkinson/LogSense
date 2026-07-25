using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.DTOs
{
    public class CreateLogEntryRequest
    {
        public string Level { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public string? Exception { get; set; }
    }
}
