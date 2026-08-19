using LogSense.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.Interfaces
{
    public interface IRagService
    {
        Task<LogAnalysisResponse> InvestigateAsync(
            string query,
            int resultCount = 5);
    }
}
