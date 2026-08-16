using LogSense.Application.Interfaces;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Infrastructure.Services
{
    public class LogEmbeddingService : ILogEmbeddingService
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>>
            _embeddingGenerator;

        public LogEmbeddingService(
            IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            _embeddingGenerator = embeddingGenerator;
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(
                    "Text is required to generate an embedding.",
                    nameof(text));
            }

            var embedding =
                await _embeddingGenerator.GenerateAsync(text);

            return embedding.Vector.ToArray();
        }
    }
}
