using LogSense.Application.Interfaces;
using LogSense.Domain.Entities;
using OpenSearch.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Infrastructure.Services
{
    public class OpenSearchService : IOpenSearchService
    {

        private readonly IOpenSearchClient _openSearchClient;

        public OpenSearchService(IOpenSearchClient openSearchClient)
        {
            _openSearchClient = openSearchClient;
        }

        public async Task IndexLogEntryAsync(LogEntry logEntry)
        {
            var response = await _openSearchClient.IndexDocumentAsync(logEntry);

            if (!response.IsValid)
            {
                throw new Exception(
                    $"Failed to index log entry in OpenSearch: {response.DebugInformation}");
            }
        }

        public async Task<List<LogEntry>> SearchLogsAsync(string query)
        {
            var response = await _openSearchClient.SearchAsync<LogEntry>(s => s
                    .Query(q => q
                        .MultiMatch(m => m
                            .Query(query)
                            .Fields(f => f
                                .Field(x => x.Message)
                                .Field(x => x.Exception)
                                .Field(x => x.Source)
                            )
                        )
                    )
                );

            if (!response.IsValid)
            {
                throw new Exception(
                    $"Failed to search logs in OpenSearch: {response.DebugInformation}");
            }

            return response.Documents.ToList();
        }
    }
}
