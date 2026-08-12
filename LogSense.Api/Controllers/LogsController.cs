using LogSense.Application.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using LogSense.Infrastructure.Persistence;
using LogSense.Domain.Entities;
using LogSense.Application.Interfaces;

namespace LogSense.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLogEntryRequest request)
        {
            await _logService.CreateLogAsync(request);

            return Created();
        }

        [HttpGet]
        public async Task<ActionResult<List<LogEntryResponse>>> GetAllLogEntries([FromQuery] string? level)
        {
            List<LogEntryResponse> logEntries;

            if (!string.IsNullOrWhiteSpace(level))
            {
                logEntries = await _logService.GetLogEntriesByLevelAsync(level);
            }
            else
            {
                logEntries = await _logService.GetAllLogsAsync();
            }

            return Ok(logEntries);
        }
    }
}
