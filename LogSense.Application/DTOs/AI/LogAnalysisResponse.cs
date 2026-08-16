using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.DTOs.AI
{
    public class LogAnalysisResponse
    {
        public string Summary { get; set; } = string.Empty;

        public string LikelyRootCause { get; set; } = string.Empty;

        public string Severity { get; set; } = string.Empty;

        public string AffectedService { get; set; } = string.Empty;

        public List<string> RecommendedActions { get; set; } = [];
    }
}
