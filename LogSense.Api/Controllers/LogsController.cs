using LogSense.Application.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using LogSense.Infrastructure.Persistence;
using LogSense.Domain.Entities;

namespace LogSense.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class LogsController : ControllerBase
    {
        private readonly LogSenseDbContext _context;

        public LogsController(LogSenseDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLogEntryRequest request)
        {
            var logEntry = new LogEntry
            {
                Level = request.Level,
                Message = request.Message,
                Source = request.Source,
                Exception = request.Exception,
                Timestamp =  DateTime.UtcNow
            };

            _context.LogEntries.Add(logEntry);
            await _context.SaveChangesAsync();

            return Created();
        }
    }
}
