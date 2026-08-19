using LogSense.Application.DTOs.AI;
using LogSense.Application.Exceptions;
using LogSense.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace LogSense.Application.Services
{
    public class RagService : IRagService
    {
        private readonly IOpenSearchService _openSearchService;
        private readonly ILogAnalysisService _logAnalysisService;
        private readonly ILogger<RagService> _logger;

        public RagService(
            IOpenSearchService openSearchService,
            ILogAnalysisService logAnalysisService,
            ILogger<RagService> logger)
        {
            _openSearchService = openSearchService;
            _logAnalysisService = logAnalysisService;
            _logger = logger;
        }

        public async Task<LogAnalysisResponse> InvestigateAsync(
            string query,
            int resultCount = 2)
        {
            _logger.LogInformation("RAG: starting semantic retrieval");

            var relevantLogs =
                await _openSearchService.SemanticSearchAsync(
                    query,
                    resultCount);

            _logger.LogInformation(
                "RAG: retrieved {Count} logs",
                relevantLogs.Count);

            foreach (var log in relevantLogs)
            {
                _logger.LogInformation(
                    "RAG retrieved: {Source} | {Message} | {Exception}",
                    log.Source,
                    log.Message,
                    log.Exception);
            }

            if (relevantLogs.Count == 0)
            {
                throw new InvalidOperationException(
                    "No relevant logs were found for the investigation.");
            }

            try
            {
                _logger.LogInformation("RAG: starting LLM analysis");

                var result =
                    await _logAnalysisService.AnalyseLogsAsync(
                        relevantLogs,
                        query);

                _logger.LogInformation("RAG: LLM analysis completed");

                return result;
            }
            catch (AiGroundingException ex)
            {
                _logger.LogWarning(
                    ex,
                    "RAG: AI response failed grounding validation. " +
                    "Returning evidence-based fallback.");

                var topLog = relevantLogs.First();

                return new LogAnalysisResponse
                {
                    Summary = topLog.Message,

                    LikelyRootCause =
                        string.IsNullOrWhiteSpace(topLog.Exception)
                            ? "Uncertain based on available logs"
                            : topLog.Exception,

                    Severity = MapSeverity(topLog.Level),

                    AffectedService = topLog.Source,

                    RecommendedActions = new List<string>
                    {
                        $"Investigate {topLog.Source}.",
                        "Review related logs for the same service and time period.",
                        "Check the health and availability of relevant external dependencies."
                    }
                };
            }
        }

        private static string MapSeverity(string? level)
        {
            return level?.ToLowerInvariant() switch
            {
                "critical" => "Critical",
                "fatal" => "Critical",
                "error" => "High",
                "warning" => "Medium",
                "warn" => "Medium",
                "information" => "Low",
                "info" => "Low",
                "debug" => "Low",
                _ => "Medium"
            };
        }
    }
}