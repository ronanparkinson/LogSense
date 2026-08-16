using LogSense.Application.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using LogSense.Infrastructure.Persistence;
using LogSense.Domain.Entities;
using LogSense.Application.Interfaces;
using LogSense.Application.DTOs.AI;
using LogSense.Infrastructure.Services;

namespace LogSense.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly ILogAnalysisService _logAnalysisService;

        public LogsController(ILogService logService, ILogAnalysisService logAnalysisService)
        {
            _logService = logService;
            _logAnalysisService = logAnalysisService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLogEntryRequest request)
        {
            await _logService.CreateLogAsync(request);

            return Created();
        }

        [HttpGet]
        public async Task<ActionResult<List<LogEntryResponse>>> GetLogEntries(
            [FromQuery] LogQueryParameters parameters)
        {
            List<LogEntryResponse> logEntries =
                await _logService.QueryLogsAsync(parameters);

            return Ok(logEntries);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchLogs([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query is required.");
            }

            var logs = await _logService.SearchLogsAsync(query);

            return Ok(logs);
        }

        [HttpGet("analyse")]
        public async Task<ActionResult<LogAnalysisResponse>> AnalyseLogs([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query is required.");
            }

            List<LogEntryResponse> searchResults =
                await _logService.SearchLogsAsync(query);

            if (searchResults.Count == 0)
            {
                return NotFound("No matching logs found.");
            }

            List<LogEntry> logEntries = searchResults
                .Select(log => new LogEntry
                {
                    Id = log.Id,
                    Timestamp = log.Timestamp,
                    Level = log.Level,
                    Message = log.Message,
                    Source = log.Source,
                    CorrelationId = log.CorrelationId,
                    Exception = log.Exception
                })
            .ToList();

            LogAnalysisResponse analysis =
                await _logAnalysisService.AnalyseLogsAsync(logEntries);

            return Ok(analysis);
        }
    }
}
