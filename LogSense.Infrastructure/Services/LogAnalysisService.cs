using LogSense.Application.DTOs.AI;
using LogSense.Application.Exceptions;
using LogSense.Application.Interfaces;
using LogSense.Domain.Entities;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LogSense.Infrastructure.Services
{
    public class LogAnalysisService : ILogAnalysisService
    {
        private readonly IChatClient _chatClient;
        private readonly ILogger<LogAnalysisService> _logger;


        public LogAnalysisService(IChatClient chatClient, ILogger<LogAnalysisService> logger)
        {
            _chatClient = chatClient;
            _logger = logger;
        }

        public async Task<LogAnalysisResponse> AnalyseLogsAsync(
            List<LogEntry> logEntries, string? investigationQuery = null)
        {
            string logs = string.Join(
                    Environment.NewLine,
                    logEntries.Select(log =>
                        $"Timestamp: {log.Timestamp:u} | " +
                        $"Level: {log.Level} | " +
                        $"Source: {log.Source} | " +
                        $"Message: {log.Message} | " +
                        $"Exception: {log.Exception ?? "None"}"));

            _logger.LogInformation(
                "Investigation query passed to analysis: {Query}",
                investigationQuery);

            var allowedServices = logEntries
                .Select(log => log.Source)
                .Where(source => !string.IsNullOrWhiteSpace(source))
                .Distinct()
                .ToList();

            string prompt = $$"""
                You are analysing application logs for an incident investigation.

                Investigation question:
                {investigationQuery ?? "General incident analysis"}

                The only services present in the supplied evidence are:

                {string.Join(", ", allowedServices)}

                The affectedService field MUST exactly match one of those service names.
                Never invent a service name that does not appear in the supplied logs.

                Base your analysis only on the supplied log entries.

                Treat explicit error messages and exceptions in the logs as valid evidence.
                If a log explicitly states that a service failed because an external dependency
                did not respond, report that as the likely cause for that service.

                Do not assume that separate errors share the same root cause unless the logs
                contain evidence linking them.

                If multiple unrelated errors are present, describe them as separate incidents.

                Do not invent causes, dependencies, or correlations that are not present
                in the logs.

                Return a JSON object with exactly these fields:

                summary: non-empty concise string
                likelyRootCause: non-empty concise string
                severity: one of "Low", "Medium", "High", "Critical"
                affectedService: non-empty string
                recommendedActions: array of 2 to 4 concise strings

                Use "Uncertain based on available logs" only when the supplied logs genuinely
                contain no evidence indicating a likely cause.

                Logs:
                {logs} 
                """;

            try
            {
                var options = new ChatOptions
                {
                    Temperature = 0.1f,
                    MaxOutputTokens = 300,
                    AdditionalProperties = new AdditionalPropertiesDictionary
                    {
                        ["think"] = false
                    }
                };

                // Get plain text from Qwen instead of using
                // Microsoft.Extensions.AI structured output
                var response =
                    await _chatClient.GetResponseAsync(
                        prompt,
                        options);

                // Get the raw text returned by Qwen
                var rawJson = response.Text;

                _logger.LogInformation(
                    "Raw LLM response before JSON cleanup: {Response}",
                    rawJson);

                // Clean up markdown code fences that the LLM may add
                rawJson = rawJson.Trim();

                if (rawJson.StartsWith("```json"))
                {
                    rawJson = rawJson
                        .Replace("```json", "")
                        .Replace("```", "")
                        .Trim();
                }
                else if (rawJson.StartsWith("```"))
                {
                    rawJson = rawJson
                        .Replace("```", "")
                        .Trim();
                }

                _logger.LogWarning(
                    "RAW LLM RESPONSE: {RawResponse}",
                    rawJson);

                // Convert the cleaned JSON into LogAnalysisResponse
                var result =
                    JsonSerializer.Deserialize<LogAnalysisResponse>(
                        rawJson,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (result == null)
                {
                    throw new AiGroundingException(
                        "AI response could not be parsed.");
                }

                // Only allow service names that actually exist
                // in the retrieved evidence
                allowedServices = logEntries
                    .Select(log => log.Source)
                    .Where(source => !string.IsNullOrWhiteSpace(source))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (string.IsNullOrWhiteSpace(result.AffectedService) ||
                    !allowedServices.Contains(
                        result.AffectedService,
                        StringComparer.OrdinalIgnoreCase))
                {
                    throw new AiGroundingException(
                        $"AI returned unsupported service '{result.AffectedService}'.");
                }

                return result;
            }
            catch (AiGroundingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "AI log analysis failed. Ensure the local Ollama service and configured model are available.",
                    ex);
            }
        }
    }
}
