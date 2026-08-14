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
    }
}
