using LogSense.Application.DTOs.AI;
using LogSense.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.Interfaces
{
    public interface ILogAnalysisService
    {
        Task<LogAnalysisResponse> AnalyseLogsAsync(
            List<LogEntry> logEntries);
    }
}
