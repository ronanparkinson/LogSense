using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.DTOs
{
    public class CreateLogEntryRequest
    {
        [Required]
        [StringLength(20)]
        public string Level { get; set; } = string.Empty;

        [Required]
        [StringLength(8000)]
        public string Message { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Source { get; set; } = string.Empty;

        public string? Exception { get; set; }
    }
}
