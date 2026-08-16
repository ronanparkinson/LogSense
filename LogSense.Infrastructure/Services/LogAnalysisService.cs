using LogSense.Application.DTOs.AI;
using LogSense.Application.Interfaces;
using LogSense.Domain.Entities;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Infrastructure.Services
{
    public class LogAnalysisService : ILogAnalysisService
    {
        private readonly IChatClient _chatClient;

        public LogAnalysisService(IChatClient chatClient)
        {
            _chatClient = chatClient;
        }

        public async Task<LogAnalysisResponse> AnalyseLogsAsync(
            List<LogEntry> logEntries)
        {
            string logs = string.Join(
                    Environment.NewLine,
                    logEntries.Select(log =>
                        $"Timestamp: {log.Timestamp:u} | " +
                        $"Level: {log.Level} | " +
                        $"Source: {log.Source} | " +
                        $"Message: {log.Message} | " +
                        $"Exception: {log.Exception ?? "None"}"));

            string prompt = $"""
                You are analysing application logs for an incident investigation.

                Analyse only the evidence contained in the supplied logs.
                Do not invent facts that are not supported by the logs.

                Return:
                - a concise summary
                - the most likely root cause
                - severity: Low, Medium, High, or Critical
                - the affected service
                - practical recommended actions

                If the evidence is insufficient to establish a root cause,
                clearly state that the root cause is uncertain.

                Logs:
                {logs}
                """;

            var response =
                await _chatClient.GetResponseAsync<LogAnalysisResponse>(prompt);

            return response.Result;
        }
    }
}
