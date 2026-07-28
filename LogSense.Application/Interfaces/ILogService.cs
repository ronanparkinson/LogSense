using LogSense.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.Interfaces
{
    public interface ILogService
    {
        Task CreateLogAsync(CreateLogEntryRequest createLogEntryRequest);
    }
}
