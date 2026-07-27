using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSense.Application.DTOs
{
    public class ErrorResponse
    {
        [Required]
        public string Message { get; set; }
    }
}
