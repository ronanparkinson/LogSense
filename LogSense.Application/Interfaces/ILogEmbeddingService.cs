using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.Interfaces
{
    public interface ILogEmbeddingService
    {
        Task<float[]> GenerateEmbeddingAsync(string text);
    }
}
