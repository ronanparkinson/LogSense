using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.DTOs
{
    public class LogQueryParameters
    {
        public string? Level { get; set; }

        public string? Source { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
