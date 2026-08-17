using LogSense.Application.Interfaces;
using LogSense.Domain.Entities;
using LogSense.Infrastructure.Models;
using Microsoft.Extensions.Logging;
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
        private readonly ILogEmbeddingService _logEmbeddingService;
        private readonly ILogger<OpenSearchService> _logger;

        public OpenSearchService(IOpenSearchClient openSearchClient, ILogEmbeddingService logEmbeddingService, ILogger<OpenSearchService> logger)
        {
            _openSearchClient = openSearchClient;
            _logEmbeddingService = logEmbeddingService;
            _logger = logger;
        }

        public async Task IndexLogEntryAsync(LogEntry logEntry)
        {
            string embeddingText =
                $"Source: {logEntry.Source}. " +
                $"Level: {logEntry.Level}. " +
                $"Message: {logEntry.Message}. " +
                $"Exception: {logEntry.Exception ?? "None"}.";

            float[] embedding =
                await _logEmbeddingService.GenerateEmbeddingAsync(embeddingText);

            var document = new OpenSearchLogDocument
            {
                Id = logEntry.Id,
                Timestamp = logEntry.Timestamp,
                Level = logEntry.Level,
                Message = logEntry.Message,
                Source = logEntry.Source,
                CorrelationId = logEntry.CorrelationId,
                Exception = logEntry.Exception,
                Embedding = embedding
            };

            var response =
                await _openSearchClient.IndexDocumentAsync(document);

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
                .Sort(sort => sort
                    .Descending(x => x.Timestamp)
                )
            );

            if (!response.IsValid)
            {
                throw new Exception(
                    $"Failed to search logs in OpenSearch: {response.DebugInformation}");
            }

            return response.Documents.ToList();
        }

        public async Task<List<LogEntry>> SemanticSearchAsync(string query, int resultCount = 5)
        {
            _logger.LogInformation("Generating semantic-search embedding");

            float[] queryEmbedding =
                await _logEmbeddingService.GenerateEmbeddingAsync(query);

            _logger.LogInformation(
                "Embedding generated with {Dimensions} dimensions",
                queryEmbedding.Length);

            _logger.LogInformation("Sending k-NN query to OpenSearch");

            var response =
                await _openSearchClient.SearchAsync<OpenSearchLogDocument>(s => s
                    .Size(resultCount)
                    .Source(src => src
                        .Excludes(e => e
                            .Field(f => f.Embedding)
                        )
                    )
                    .Query(q => q
                        .Knn(k => k
                            .Field(f => f.Embedding)
                            .Vector(queryEmbedding)
                            .K(resultCount)
                        )
                    )
                );

            _logger.LogInformation("OpenSearch k-NN response received");

            if (!response.IsValid)
            {
                throw new Exception(
                    $"Semantic search failed in OpenSearch: {response.DebugInformation}");
            }

            return response.Documents
                .Select(document => new LogEntry
                {
                    Id = document.Id,
                    Timestamp = document.Timestamp,
                    Level = document.Level,
                    Message = document.Message,
                    Source = document.Source,
                    CorrelationId = document.CorrelationId,
                    Exception = document.Exception
                })
                .ToList();
        }
    }
}
